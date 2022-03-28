<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Mom.master" Inherits="AddUser" Title="Add User || MOM" ValidateRequest="false" CodeBehind="AddUser.aspx.cs" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <%--<link href="Design/css/grid.css" rel="stylesheet" />--%>
    <link href="Design/css/grid.css" rel="stylesheet" />
    <!--File Upload Control-->
    <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection">
    <script src="js/jquery.sumoselect.min.js"></script>
    <script type="text/javascript" src="js/jquery.geocomplete.js"></script>
    <link href="Styles/sumoselect.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">

        function cancel() {
            window.parent.document.getElementById('ctl00_ContentPlaceHolder1_hideModalPopupViaServer').click();
        }
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
                return o.value.dtxtScrapFactor = 0;
            }
        }
        function AddNewMerchant(dropdown) {
            if (dropdown.selectedIndex == 1) {
                TogglePopUp();
            }
        }
        function showDecimalVal(obj) {
            if (!isNaN(parseFloat(document.getElementById(obj.id).value))) {
                document.getElementById(obj.id).value = parseFloat(document.getElementById(obj.id).value).toFixed(2);
            }
        }
        function RemoveWageGridRow(Gridview) {

            $("#" + Gridview).find('tr').each(function () {
                var $tr = $(this);
                $tr.find('input[type="checkbox"]:checked').each(function () {

                    if ($("#" + Gridview).find('tr').length > 2) {
                        $(this).closest('tr').remove();
                    }
                    else {
                        $(this).closest('tr').find('input:text').val('');
                        $(this).closest('tr').find('.dropdown-content').val('0');
                    }
                });
            });
        }
        function CheckDelete() {
            var result = false;
            $("#<%=gvWagePayRate.ClientID%> tr").each(function () {
                var checkBox = $(this).find("input[type='checkbox']");
                if (checkBox.is(":checked")) {
                    result = true;
                }
            });

            if (result == true) {
                itemJSON();
                return confirm('Do you really want to delete this Wage category ?');
            }
            else {
                alert('Please select an wage category to delete.')
                return false;
            }
        }
        function CheckCopyRate() {
            var result = 0;
            $("#<%=gvWagePayRate.ClientID%> tr").each(function () {
                var checkBox = $(this).find("input[type='checkbox']");
                if (checkBox.is(":checked")) {
                    result++;
                }
            });

            if (result == 1) {
                itemJSON();
                return confirm('Do you really want to copy the selected rates to all?');
            } if (result > 1) {
                alert('Please select a single wage category to copy.');
                return false;
            }
            else {
                alert('Please select an wage category to copy.');
                return false;
            }
        }
        function InitializeGrids(Gridview) {

            var rowone = $("#" + Gridview).find('tr').eq(1);
            $("input", rowone).each(function () {
                $(this).blur();
            });
        }
        function TogglePopUp() {
            //debugger;
            var pnlMerchant = document.getElementById('<%= pnlMerchant.ClientID %>');
            if (pnlMerchant.style.display == 'block') {
                $find("programmaticModalPopupBehavior").hide();
                return false;
            } else {
                $find("programmaticModalPopupBehavior").show();
                return false;
            }
        }
        function opensup1() {
            javascript: window.open("adduser?sup=1", "_blank", "width=941,height=700");
            Materialize.updateTextFields();
        }

        $(document).ready(function () {
            Materialize.updateTextFields();

            $('#<%=lstSelectCompany.ClientID%>').SumoSelect({ selectAll: true });

            InitializeGrids('<%=gvWagePayRate.ClientID%>');

            $("#ctl00_ContentPlaceHolder1_TabContainer2_tbWage_gvWagePayRate input:text").focus(function () { $(this).select(); }); // to update rates


        });

        $(document).ready(function () {

            if (document.getElementById('<%= chkSuper.ClientID%>').checked) {
                //alert('Checked');
                $("#userlist").css("display", "block");
            }
            else {
                //alert('Unchecked');
                $("#userlist").css("display", "none");
            }


        });


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
        function itemJSON() {

            var _txtHireDt = $("#<%=txtHireDt.ClientID%>").val();

            // alert(_txtHireDt.length);

            if (_txtHireDt.length < 1) {

                alert('Please select Date of Hiring.');

                document.getElementById('apaymentclick').click();
            }

            var rawData = $('#<%=gvWagePayRate.ClientID%>').serializeFormJSON();

            var formData = JSON.stringify(rawData);

            $('#<%=hdnWageRate.ClientID%>').val(formData);

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

        function initialize() {
            var address = new google.maps.LatLng(document.getElementById('<%= txtLatitude.ClientID %>').value, document.getElementById('<%= txtLongitude.ClientID %>').value);
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
        $(document).ready(function () {
            $(function () {
                $("#<%= txtAddress.ClientID %>").geocomplete({
                    map: false,
                    details: "#divmain",
                    types: ["geocode", "establishment"],
                    address: "#<%= txtAddress.ClientID %>",
                    city: "#<%= txtCity.ClientID %>",
                    state: "#<%= ddlState.ClientID %>",
                    zip: "#<%= txtZip.ClientID %>",
                    lat: "#<%= txtLatitude.ClientID %>",
                    lng: "#<%= txtLongitude.ClientID %>"
                }).bind("geocode:result", function (event, result) {

                    var countryCode = "", city = "", cityAlt = "", getCountry = "";
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
                    //if (cityAlt.length > 2)
                    //    for (var i = 0; i < result.address_components.length; i++) {
                    //        var addr = result.address_components[i];
                    //        if (addr.types[0] == 'administrative_area_level_2')
                    //            cityAlt = addr.short_name;
                    //    }
                    $("#<%=ddlCountry.ClientID%>").val(getCountry);
                    $("#<%=txtZip.ClientID%>").val(countryCode);
                    $("#<%=ddlState.ClientID%>").val(cityAlt);
                    $("#<%=txtCity.ClientID%>").val(city);
                    Materialize.updateTextFields();


                    if (getCountry == 'US') {
                        $('#<%=txtSSNSIN.ClientID%>').mask("999-99-9999");
                        $('#<%=txtSSNSIN.ClientID%>').attr("placeholder", "xxx-xx-xxxx");
                    }
                    else {
                        $('#<%=txtSSNSIN.ClientID%>').mask("999-999-999");
                        $('#<%=txtSSNSIN.ClientID%>').attr("placeholder", "xxx-xxx-xxx");
                    }
                    Materialize.updateTextFields();
                });

                initialize();
            });

            $('#<%=txtTelephone.ClientID%>').mask("(999) 999-9999");
            $('#<%=txtCell.ClientID%>').mask("(999) 999-9999");

        });

        function activeLabel(value) {
            var txtHours = $(".txtHours").val();
            var lblHours = $(".lblHours");
            var txtAmount = $(".txtAmount").val();
            var lblAmount = $(".lblAmount");
            if (value === "Hours") {
                if (txtHours != undefined) {
                    if (txtHours.length === 0) {
                        lblHours.removeClass("active");
                    }
                    else {
                        lblHours.addClass("active");
                    }
                }
            }
            else if (value === "Amount") {
                if (txtAmount != undefined) {
                    if (txtAmount.length === 0) {
                        lblAmount.removeClass("active");
                    }
                    else {
                        lblAmount.addClass("active");
                    }
                }
            }
        }

        function updateField() {
            var input_selector = 'input[type=text], input[type=password], input[type=email], input[type=url], input[type=tel], input[type=number], input[type=search], textarea';
            $(input_selector).each(function (index, element) {
                if ($(element).val().length > 0 || $(this).attr('placeholder') !== undefined || $(element)[0].validity.badInput === true) {
                    $(this).siblings('label').addClass('active');
                }
                else {
                    $(this).siblings('label, i').removeClass('active');
                }
            });
        };
        function checkCustomerModule() {
            // Customer Module
            if (document.getElementById('<%= chkCustomeradd.ClientID%>').checked || document.getElementById('<%= chkCustomeredit.ClientID%>').checked || document.getElementById('<%= chkCustomerdelete.ClientID%>').checked) {
                document.getElementById('<%=chkCustomerview.ClientID %>').checked = true;
                document.getElementById('<%=chkCustomerModule.ClientID %>').checked = true;
            }
            if (document.getElementById('<%= chkLocationadd.ClientID%>').checked || document.getElementById('<%= chkLocationedit.ClientID%>').checked || document.getElementById('<%= chkLocationdelete.ClientID%>').checked) {
                document.getElementById('<%=chkLocationview.ClientID %>').checked = true;
                document.getElementById('<%=chkCustomerModule.ClientID %>').checked = true;
            }
            if (document.getElementById('<%= chkEquipmentsadd.ClientID%>').checked || document.getElementById('<%= chkEquipmentsedit.ClientID%>').checked || document.getElementById('<%= chkEquipmentsdelete.ClientID%>').checked) {
                document.getElementById('<%=chkEquipmentsview.ClientID %>').checked = true;
                document.getElementById('<%=chkCustomerModule.ClientID %>').checked = true;
            }
            if (document.getElementById('<%= chkReceivePaymentAdd.ClientID%>').checked || document.getElementById('<%= chkReceivePaymentEdit.ClientID%>').checked || document.getElementById('<%= chkReceivePaymentDelete.ClientID%>').checked) {
                document.getElementById('<%=chkReceivePaymentView.ClientID %>').checked = true;
                document.getElementById('<%=chkCustomerModule.ClientID %>').checked = true;
            }
<%--            if (document.getElementById('<%= chkOnlinePaymentApprove.ClientID%>').checked) {
                document.getElementById('<%=chkOnlinePaymentView.ClientID %>').checked = true;
                document.getElementById('<%=chkCustomerModule.ClientID %>').checked = true;
            }--%>
            if (document.getElementById('<%= chkMakeDepositAdd.ClientID%>').checked || document.getElementById('<%= chkMakeDepositEdit.ClientID%>').checked || document.getElementById('<%= chkMakeDepositDelete.ClientID%>').checked) {
                document.getElementById('<%=chkMakeDepositView.ClientID %>').checked = true;
                document.getElementById('<%=chkCustomerModule.ClientID %>').checked = true;
            }
            if (document.getElementById('<%= chkCollectionsAdd.ClientID%>').checked || document.getElementById('<%= chkCollectionsEdit.ClientID%>').checked || document.getElementById('<%= chkCollectionsDelete.ClientID%>').checked || document.getElementById('<%= chkCollectionsReport.ClientID%>').checked) {
                document.getElementById('<%=chkCollectionsView.ClientID %>').checked = true;
                document.getElementById('<%=chkCustomerModule.ClientID %>').checked = true;
            }

            // AP Module 
            if (document.getElementById('<%= chkVendorsAdd.ClientID%>').checked || document.getElementById('<%= chkVendorsEdit.ClientID%>').checked || document.getElementById('<%= chkVendorsDelete.ClientID%>').checked) {
                document.getElementById('<%=chkVendorsView.ClientID %>').checked = true;
                document.getElementById('<%=chkAccountPayable.ClientID %>').checked = true;
            }
            if (document.getElementById('<%= chkAddBills.ClientID%>').checked || document.getElementById('<%= chkEditBills.ClientID%>').checked || document.getElementById('<%= chkDeleteBills.ClientID%>').checked) {
                document.getElementById('<%=chkViewBills.ClientID %>').checked = true;
                document.getElementById('<%=chkAccountPayable.ClientID %>').checked = true;
            }
            if (document.getElementById('<%= chkAddManageChecks.ClientID%>').checked || document.getElementById('<%= chkEditManageChecks.ClientID%>').checked || document.getElementById('<%= chkDeleteManageChecks.ClientID%>').checked || document.getElementById('<%= chkShowBankBalances.ClientID%>').checked) {
                document.getElementById('<%=chkViewManageChecks.ClientID %>').checked = true;
                document.getElementById('<%=chkAccountPayable.ClientID %>').checked = true;
            }

            // Financial Module 
            if (document.getElementById('<%= chkChartAdd.ClientID%>').checked || document.getElementById('<%= chkChartEdit.ClientID%>').checked || document.getElementById('<%= chkChartDelete.ClientID%>').checked) {
                document.getElementById('<%=chkChartView.ClientID %>').checked = true;
                document.getElementById('<%=chkFinancialmodule.ClientID %>').checked = true;
            }
            if (document.getElementById('<%= chkJournalEntryAdd.ClientID%>').checked || document.getElementById('<%= chkJournalEntryEdit.ClientID%>').checked || document.getElementById('<%= chkJournalEntryDelete.ClientID%>').checked) {
                document.getElementById('<%=chkJournalEntryView.ClientID %>').checked = true;
                document.getElementById('<%=chkFinancialmodule.ClientID %>').checked = true;
            }
            if (document.getElementById('<%= chkBankAdd.ClientID%>').checked || document.getElementById('<%= chkBankEdit.ClientID%>').checked || document.getElementById('<%= chkBankDelete.ClientID%>').checked) {
                document.getElementById('<%=chkBankView.ClientID %>').checked = true;
                document.getElementById('<%=chkFinancialmodule.ClientID %>').checked = true;
            }

            // Billing Module 
            if (document.getElementById('<%= chkInvoicesAdd.ClientID%>').checked || document.getElementById('<%= chkInvoicesEdit.ClientID%>').checked || document.getElementById('<%= chkInvoicesDelete.ClientID%>').checked) {
                document.getElementById('<%=chkInvoicesView.ClientID %>').checked = true;
                document.getElementById('<%=chkBillingmodule.ClientID %>').checked = true;
            }
            if (document.getElementById('<%= chkBillingcodesAdd.ClientID%>').checked || document.getElementById('<%= chkBillingcodesEdit.ClientID%>').checked || document.getElementById('<%= chkBillingcodesDelete.ClientID%>').checked) {
                document.getElementById('<%=chkBillingcodesView.ClientID %>').checked = true;
                document.getElementById('<%=chkBillingmodule.ClientID %>').checked = true;
            }
            // purchasing module 
            if (document.getElementById('<%= chkPOAdd.ClientID%>').checked || document.getElementById('<%= chkPOEdit.ClientID%>').checked || document.getElementById('<%= chkPODelete.ClientID%>').checked) {
                document.getElementById('<%=chkPOView.ClientID %>').checked = true;
                document.getElementById('<%=chkPurchasingmodule.ClientID %>').checked = true;
            }
            if (document.getElementById('<%= chkRPOAdd.ClientID%>').checked || document.getElementById('<%= chkRPOEdit.ClientID%>').checked || document.getElementById('<%= chkRPODelete.ClientID%>').checked) {
                document.getElementById('<%=chkRPOView.ClientID %>').checked = true;
                document.getElementById('<%=chkPurchasingmodule.ClientID %>').checked = true;
            }
            // Recurring Module 
            if (document.getElementById('<%= chkRecContractsAdd.ClientID%>').checked || document.getElementById('<%= chkRecContractsEdit.ClientID%>').checked || document.getElementById('<%= chkRecContractsDelete.ClientID%>').checked) {
                document.getElementById('<%=chkRecContractsView.ClientID %>').checked = true;
                document.getElementById('<%=chkRecurring.ClientID %>').checked = true;
            }
            if (document.getElementById('<%= chkRecInvoicesAdd.ClientID%>').checked || document.getElementById('<%= chkRecInvoicesDelete.ClientID%>').checked) {
                document.getElementById('<%=chkRecInvoicesView.ClientID %>').checked = true;
                document.getElementById('<%=chkRecurring.ClientID %>').checked = true;
            }
            if (document.getElementById('<%= chkRecTicketsAdd.ClientID%>').checked || document.getElementById('<%= chkRecTicketsDelete.ClientID%>').checked) {
                document.getElementById('<%=chkRecTicketsView.ClientID %>').checked = true;
                document.getElementById('<%=chkRecurring.ClientID %>').checked = true;
            }
            if (document.getElementById('<%= chkSafetyTestsAdd.ClientID%>').checked || document.getElementById('<%= chkSafetyTestsEdit.ClientID%>').checked || document.getElementById('<%= chkSafetyTestsDelete.ClientID%>').checked) {
                document.getElementById('<%=chkSafetyTestsView.ClientID %>').checked = true;
                document.getElementById('<%=chkRecurring.ClientID %>').checked = true;
            }
            if (document.getElementById('<%= chkRenewEscalateAdd.ClientID%>').checked) {
                document.getElementById('<%=chkRenewEscalateView.ClientID %>').checked = true;
                document.getElementById('<%=chkRecurring.ClientID %>').checked = true;
            }

            // Schedule Module 

            if (document.getElementById('<%= chkTimesheetadd.ClientID%>').checked || document.getElementById('<%= chkTimesheetedit.ClientID%>').checked || document.getElementById('<%= chkTimesheetdelete.ClientID%>').checked) {
                document.getElementById('<%=chkTimesheetview.ClientID %>').checked = true;
                document.getElementById('<%=chkSchedule.ClientID %>').checked = true;
            }
            if (document.getElementById('<%= chkETimesheetadd.ClientID%>').checked || document.getElementById('<%= chkTimesheetedit.ClientID%>').checked || document.getElementById('<%= chkETimesheetdelete.ClientID%>').checked) {
                document.getElementById('<%=chkETimesheetview.ClientID %>').checked = true;
                document.getElementById('<%=chkSchedule.ClientID %>').checked = true;
            }
            if (document.getElementById('<%= chkMapAdd.ClientID%>').checked || document.getElementById('<%= chkMapEdit.ClientID%>').checked || document.getElementById('<%= chkMapDelete.ClientID%>').checked) {
                document.getElementById('<%=chkMapView.ClientID %>').checked = true;
                document.getElementById('<%=chkSchedule.ClientID %>').checked = true;
            }
            if (document.getElementById('<%= chkRouteBuilderAdd.ClientID%>').checked || document.getElementById('<%= chkRouteBuilderEdit.ClientID%>').checked || document.getElementById('<%= chkRouteBuilderDelete.ClientID%>').checked) {
                document.getElementById('<%=chkRouteBuilderView.ClientID %>').checked = true;
                document.getElementById('<%=chkSchedule.ClientID %>').checked = true;
            }
            // Sales Module 
            if (document.getElementById('<%= chkLeadAdd.ClientID%>').checked || document.getElementById('<%= chkLeadEdit.ClientID%>').checked || document.getElementById('<%= chkLeadDelete.ClientID%>').checked || document.getElementById('<%= chkLeadReport.ClientID%>').checked) {
                document.getElementById('<%=chkLeadView.ClientID %>').checked = true;
                document.getElementById('<%=chkSalesMgr.ClientID %>').checked = true;
            }
            if (document.getElementById('<%=chkOppAdd.ClientID%>').checked || document.getElementById('<%=chkOppEdit.ClientID%>').checked || document.getElementById('<%=chkOppDelete.ClientID%>').checked || document.getElementById('<%=chkOppReport.ClientID%>').checked) {
                document.getElementById('<%=chkOppView.ClientID %>').checked = true;
                document.getElementById('<%=chkSalesMgr.ClientID %>').checked = true;
            }
            if (document.getElementById('<%=chkEstimateAdd.ClientID%>').checked || document.getElementById('<%=chkEstimateEdit.ClientID%>').checked || document.getElementById('<%=chkEstimateDelete.ClientID%>').checked || document.getElementById('<%=chkEstimateReport.ClientID%>').checked) {
                document.getElementById('<%=chkEstimateView.ClientID %>').checked = true;
                document.getElementById('<%=chkSalesMgr.ClientID %>').checked = true;
            }

            //payroll Module

            if (document.getElementById('<%= empAdd.ClientID%>').checked || document.getElementById('<%= empEdit.ClientID%>').checked || document.getElementById('<%= empEdit.ClientID%>').checked) {
                document.getElementById('<%=empView.ClientID %>').checked = true;
                document.getElementById('<%=payrollModulchck.ClientID %>').checked = true;
            }
            if (document.getElementById('<%=runAdd.ClientID%>').checked || document.getElementById('<%=runEdit.ClientID%>').checked || document.getElementById('<%=runDelete.ClientID%>').checked) {
                document.getElementById('<%=runView.ClientID %>').checked = true;
                document.getElementById('<%=payrollModulchck.ClientID %>').checked = true;
            }
            if (document.getElementById('<%=payrollchckAdd.ClientID%>').checked || document.getElementById('<%=payrollchckEdit.ClientID%>').checked || document.getElementById('<%=payrollchckDelete.ClientID%>').checked) {
                document.getElementById('<%=payrollchckView.ClientID %>').checked = true;
                document.getElementById('<%=payrollModulchck.ClientID %>').checked = true;
            }
            if (document.getElementById('<%=payrollformAdd.ClientID%>').checked || document.getElementById('<%=payrollformEdit.ClientID%>').checked || document.getElementById('<%=payrollformDelete.ClientID%>').checked) {
                document.getElementById('<%=payrollformView.ClientID %>').checked = true;
                document.getElementById('<%=payrollModulchck.ClientID %>').checked = true;
            }
            if (document.getElementById('<%=wagesadd.ClientID%>').checked || document.getElementById('<%=wagesEdit.ClientID%>').checked || document.getElementById('<%=wagesDelete.ClientID%>').checked) {
                document.getElementById('<%=wagesView.ClientID %>').checked = true;
                document.getElementById('<%=payrollModulchck.ClientID %>').checked = true;
            }

            if (document.getElementById('<%=deductionsAdd.ClientID%>').checked || document.getElementById('<%=deductionsAdd.ClientID%>').checked || document.getElementById('<%=deductionsDelete.ClientID%>').checked) {
                document.getElementById('<%=deductionsView.ClientID %>').checked = true;
                document.getElementById('<%=payrollModulchck.ClientID %>').checked = true;
            }


        }


    </script>

    <script type="text/javascript">
        function gvUsersShow(ttt) {
            debugger;
            if (ttt == true) {
                $("#userlist").css("display", "block");
            }
            else {
                $("#userlist").css("display", "none");
            }
        }
    </script>

    <script type="text/javascript">
        function hideModalPopup() {
            jQuery("#setuppopup").modal("hide");
            window.location.reload();
        }

        function RefreshScreen() {
            document.getElementById("<%=btnRefreshScreen.ClientID%>").click();
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

    
    <style type="text/css">
        .ModalPopupBG {
            background-color: black;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }

        .model-popup-body {
            background: #316b9d;
            padding: 5px;
        }
        input:disabled {
        margin: 7px 0px 10px 0px!important;
}

        .table_Permission td, th {
            padding: 3px 5px !important;
            font-size: 0.9rem;
        }

        .table_Permission tr:nth-child(odd) {
            background: #edf4fc;
        }

        .table_Permission tr:nth-child(even) {
            background: #fff;
        }

        .gvPages .rgDataDiv, .gvPage1 .rgDataDiv, .gvPage2 .rgDataDiv {
            max-height: 200px;
        }

        .text_center {
            text-align: center !important;
        }

        .iconAdd_gvWagePayRate {
            margin-left: -20px;
            margin-top: 1px;
            font-size: 2rem !important;
        }

        .gvWagePayRate .rgFilterBox {
            padding: 0 !important;
        }

        .lnkDone {
            display: none;
            padding: 0 20px !important;
        }

        .btnCustomlinks {
            float: left;
            margin-top: 5px;
            margin-right: 3px;
            margin-left: 3px;
        }

            .btnCustomlinks a {
                border: 0.5px solid #1C5FB1;
                color: #1C5FB1;
                padding: 0px 20px 5px 20px;
                border-radius: 3px;
                font-size: 0.9em;
                background-image: url(../images/accrd.gif);
                background-repeat: repeat-x;
            }

        .btnCustomEdit {
            display: block;
            height: 27px !important;
            margin-top: auto;
        }

        .btnEdit {
            display: block;
            height: 27px !important;
        }

        .chkPermission input {
            vertical-align: baseline !important;
        }

        .chkPermission label {
            vertical-align: text-top !important;
            padding: 10px !important;
            font-size: 13px !important;
        }

        .gvWagePayRate, .gvWagePayRate .rgMasterTable {
            font-family: 'Lato', sans-serif !important;
            font-size: 12.15px !important;
        }

            .gvWagePayRate .rgHeader a {
                padding: 0 !important;
            }

        .margin-top-3px {
            margin-top: 3px;
        }


        [id$='RequiredFieldValidator10_ValidatorCalloutExtender_popupTable'] {
            max-width: 162px !important;
        }

        .valiateField {
            top: -60px !important;
            width: 190px !important;
        }

            .valiateField td, .valiateField div {
                border: solid 1px Black;
                background-color: LemonChiffon;
            }

        .disableControl {
            pointer-events: none;
            color: #ccc !important;
        }

        .hdsPayRate, .hdsWage {
            border-right: 1px solid #bbb !important;
        }

        .hdsPayRate, .hdsBurdenRate {
            color: #2e6b89 !important;
            font-weight: bold !important;
        }


        .gvUsers .rgNumPart > a.rgCurrentPage {
            height: 29px !important;
            width: 29px !important;
            line-height: 30px !important;
        }

        .gvUsers .rgNumPart > a {
            line-height: 33px !important;
        }

        .gvUsers .rgIcon {
            display: inline !important;
        }

        .gvUsers .rgArrPart2 > button, .gvUsers .rgArrPart1 > button {
            background: none transparent !important;
        }

        .gvUsers [id$='_PageSizeComboBox'] > span.rcbInner {
            padding: 0 !important;
        }

            .gvUsers [id$='_PageSizeComboBox'] > span.rcbInner > input {
                border-bottom: none !important;
                padding: 1px;
            }

            .gvUsers [id$='_PageSizeComboBox'] > span.rcbInner .p-icon {
                display: inline-block !important;
            }


        .gvUsers .rgWrap > .rgPagerLabel {
            padding: 7px 7px 0 7px !important;
        }

        .gvUsers .rgInfoPart {
            margin-top: 0 !important;
        }

        .PayRate {
            background-color: #98FB98 !important;
        }

        .BurdenRate {
            background-color: #FFFFC2 !important;
        }

        [id*=RadGrid_gvLogs].RadGrid_Bootstrap .rgPagerCell .rgNumPart a, [id*=RadGrid_gvLogs] .RadGrid_Bootstrap .rgPagerCell .rgPagerButton, [id*=RadGrid_gvLogs] .RadGrid_Bootstrap .rgPagerCell .rgActionButton {
            padding-bottom: 0 !important;
            padding-top: 3px !important;
        }

        [id*=RadGrid_gvLogs].RadGrid_Bootstrap .rgPagerCell .rgPageFirst,
        [id*=RadGrid_gvLogs].RadGrid_Bootstrap .rgPagerCell .rgPagePrev,
        [id*=RadGrid_gvLogs].RadGrid_Bootstrap .rgPagerCell .rgPageNext,
        [id*=RadGrid_gvLogs].RadGrid_Bootstrap .rgPagerCell .rgPageLast {
            background-position: unset !important;
        }

        [id*=RadGrid_gvLogs].RadGrid_Bootstrap .rgPagerCell .rgArrPart1, [id*=RadGrid_gvLogs] .RadGrid_Bootstrap .rgPagerCell .rgArrPart2 {
            font-size: initial !important;
        }

        [id*=RadGrid_gvLogs].RadGrid .rgPagerCell .rgNumPart a {
            padding-top: 6px !important;
            height: 29px !important;
        }

        [id*=RadGrid_gvLogs].RadGrid .rgPagerCell .rgPagerLabel {
            padding-top: 6px !important;
        }

        [id*=RadGrid_gvLogs].RadGrid .rgPagerCell .rgInfoPart {
            padding-top: 0px !important;
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


        .txtSignBody {
            min-height: 200px;
        }

        [id$='FontName'], [id$='FontSize'] {
            display: inline-block !important;
            margin: 0 !important;
            font-size: 12px !important;
        }

        .headerCollection .rwTitleWrapper {
            padding: 0 !important;
            font-size: 1.42857em;
            font-family: inherit;
            line-height: 1em;
            color: #fff;
            text-transform: capitalize;
        }

        .headerCollection .rwTitleBar {
            background-color: #1c5fb1 !important;
            padding: 5px;
        }
    </style>

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
                /*margin-top: 0px!important;*/
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
        <img src="images/wheel.GIF" alt="Be patient..." class="lodder" />
    </div>
    <telerik:RadAjaxManager ID="RadAjaxManager_User" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="lnkDone">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvUsers" LoadingPanelID="RadAjaxLoadingPanel_gvUsers" />
                    <telerik:AjaxUpdatedControl ControlID="btnEdit" LoadingPanelID="RadAjaxLoadingPanel_gvUsers" />
                    <telerik:AjaxUpdatedControl ControlID="hfCheckEdit" LoadingPanelID="RadAjaxLoadingPanel_gvUsers" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnEdit">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvUsers" LoadingPanelID="RadAjaxLoadingPanel_gvUsers" />
                    <telerik:AjaxUpdatedControl ControlID="lnkDone" LoadingPanelID="RadAjaxLoadingPanel_gvUsers" />
                    <telerik:AjaxUpdatedControl ControlID="hfCheckEdit" LoadingPanelID="RadAjaxLoadingPanel_gvUsers" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ddlPayMethod">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="txtHours" LoadingPanelID="RadAjaxLoadingPanel_gvUsers" />
                    <telerik:AjaxUpdatedControl ControlID="txtAmount" LoadingPanelID="RadAjaxLoadingPanel_gvUsers" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnCopyRate">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvWagePayRate" LoadingPanelID="RadAjaxLoadingPanel_gvUsers" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkChk">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvWagePayRate" LoadingPanelID="RadAjaxLoadingPanel_gvUsers" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="lnkLoadLogs">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="hdnLoadLogs" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvLogs" LoadingPanelID="RadAjaxLoadingPanel_gvUsers"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ddlUserRole">
                <UpdatedControls>
                    <%--<telerik:AjaxUpdatedControl ControlID="divApplyUserRolePermission"/>--%>
                    <telerik:AjaxUpdatedControl ControlID="hdnApplyUserRolePermissionOrg" />
                    <telerik:AjaxUpdatedControl ControlID="ddlUserRole" />
                    <telerik:AjaxUpdatedControl ControlID="ddlApplyUserRolePermission" />
                    <telerik:AjaxUpdatedControl ControlID="tblPermission" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ddlApplyUserRolePermission">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tblPermission" />
                    <telerik:AjaxUpdatedControl ControlID="hdnApplyUserRolePermissionOrg" />
                    <telerik:AjaxUpdatedControl ControlID="ddlApplyUserRolePermission" />
                    <telerik:AjaxUpdatedControl ControlID="ddlUserRole" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnSubmit">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="hdnLoadLogs" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvLogs" LoadingPanelID="RadAjaxLoadingPanel_gvUsers"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="gvUserCustom"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btnRefreshScreen">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnlEmailAccount"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="pnlEmailSignature"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_EmailSigns"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btnDeleteEmailSign">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_EmailSigns"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="chkEmailAcc">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnlEmailAccount"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadPersistenceManager ID="RadPersistenceManager1" runat="server">
        <PersistenceSettings>
            <telerik:PersistenceSetting ControlID="gvWagePayRate" />
            <telerik:PersistenceSetting ControlID="gvUsers" />
        </PersistenceSettings>
    </telerik:RadPersistenceManager>
    <div style="height: 65px !important;">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title"><i class="mdi-social-person-outline"></i>&nbsp;<asp:Label ID="lblHeader" runat="server">Add User</asp:Label></div>
                                    <div class="buttonContainer">
                                    <div class="btnlinks">
                                        <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" ToolTip="Save" Text="Save"
                                            OnClientClick="itemJSON();"></asp:LinkButton>
                                        <asp:HiddenField ID="hdnLocCount" runat="server" />
                                    </div>
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
                                    <div class="rght-content cont-css" >
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
                                    <li class="accrdUserInfo"><a href="#accrdUserInfo">User Info</a></li>
                                    <li id="liSignature" runat="server"><a href="#accrdSignature">Email</a></li>
                                    <li class="accrdCred"><a href="#accrdCred">Field Worker Options</a></li>
                                    <li class="accrdPermissions"><a href="#accrdPermissions">Permissions</a></li>
                                    <li class="accrdPay"><a href="#accrdPay" id="apaymentclick">Payment</a></li>
                                    <%--<li class="accrdCustom"><a href="#accrdCustom" id="accrdCustom">Custom</a></li>--%>
                                    <li runat="server" id="liCustom"><a href="#accrdcustom" id="defaultCustom">Custom</a></li>
                                    <li id="liDocuments"><a href="#accrdDocuments">Documents</a></li>
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
                    <div class="container accordian-wrap">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <ul class="collapsible popout collapsible-accordion form-accordion-head" data-collapsible="expandable">
                                        <li>
                                            <div id="accrdUserInfo" class="collapsible-header accrd active accordian-text-custom "><i class="mdi-social-poll"></i>User Info.</div>
                                            <div class="collapsible-body">
                                                <div class="form-content-wrap">
                                                    <div class="form-content-pd">
                                                        <div class="form-section-row">
                                                            <div class="col s12">
                                                                <div class="form-section-row">
                                                                    <div class="section-ttle">User Details</div>
                                                                    <div class="form-section3">
                                                                        <div class="col s4">
                                                                            <asp:Image ID="ProfileImage" CssClass="prf-css"  runat="server" Height="100px" Width="100px" ImageUrl="images/User.png"></asp:Image>
                                                                        </div>
                                                                        <div class="col s8">
                                                                            <div class="input-field col s12">
                                                                                <div class="row">
                                                                                    <asp:TextBox ID="txtFName" TextMode="SingleLine" runat="server" CssClass="txtFName validate" MaxLength="15">
                                                                                    </asp:TextBox>
                                                                                    <label for="txtFName">First Name<span class="reqd">*</span></label>

                                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                                                        ControlToValidate="txtFName" Display="None" ErrorMessage="First Name Required"
                                                                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                    <asp:ValidatorCalloutExtender
                                                                                        ID="RequiredFieldValidator1_ValidatorCalloutExtender" CssClass="valiateField" runat="server" Enabled="True" PopupPosition="TopLeft"
                                                                                        TargetControlID="RequiredFieldValidator1">
                                                                                    </asp:ValidatorCalloutExtender>
                                                                                </div>
                                                                            </div>
                                                                            <div class="input-field col s12">
                                                                                <div class="row">
                                                                                    <label for="txtMName">Middle Name</label>

                                                                                    <asp:TextBox ID="txtMName" runat="server" MaxLength="5" CssClass="validate" ToolTip="Middle Name">
                                                                                    </asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                            <div class="input-field col s12">
                                                                                <div class="row">
                                                                                    <asp:TextBox ID="txtLName" runat="server" CssClass="txtLName validate" MaxLength="15"></asp:TextBox>
                                                                                    <label for="txtLName">Last Name<span class="reqd">*</span></label>
                                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                                                        ControlToValidate="txtLName" Display="None" ErrorMessage="Last Name Required"
                                                                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                    <asp:ValidatorCalloutExtender
                                                                                        ID="RequiredFieldValidator2_ValidatorCalloutExtender" CssClass="valiateField" runat="server" Enabled="True" PopupPosition="TopLeft"
                                                                                        TargetControlID="RequiredFieldValidator2">
                                                                                    </asp:ValidatorCalloutExtender>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <%--<div class="input-field col s12">
                                                                            <div class="row">
                                                                                <asp:TextBox ID="txtCustName" CssClass="validate" runat="server"></asp:TextBox>
                                                                                <label>Customer Name</label>
                                                                            </div>
                                                                        </div>--%>

                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <%--<input type="text" class="validate" txt/>--%>
                                                                                <asp:TextBox ID="txtUserTitle" Text="" CssClass="validate" runat="server"></asp:TextBox>
                                                                                <label>User Title</label>
                                                                            </div>
                                                                        </div>


                                                                    </div>
                                                                    <div class="form-section3-blank">
                                                                        &nbsp;
                                                                    </div>
                                                                    <div class="form-section3">
                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <label for="txtAddress">Address<span class="reqd">*</span></label>
                                                                                <textarea id="txtAddress" runat="server" class="txtAddress materialize-textarea validate" placeholder="" maxlength="100"></textarea>

                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator_Address" runat="server"
                                                                                    ControlToValidate="txtAddress" Display="None" ErrorMessage="Address Required"
                                                                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                <asp:ValidatorCalloutExtender
                                                                                    ID="ValidatorCalloutExtender_Address" CssClass="valiateField" runat="server"
                                                                                    Enabled="True" TargetControlID="RequiredFieldValidator_Address" PopupPosition="TopLeft">
                                                                                </asp:ValidatorCalloutExtender>
                                                                            </div>
                                                                        </div>

                                                                        <div class="input-field col s5">
                                                                            <div class="row">
                                                                                <asp:TextBox ID="txtCity" runat="server" MaxLength="50" CssClass="txtCity validate"></asp:TextBox>
                                                                                <label for="txtCity">City</label>
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator_City" runat="server"
                                                                                    ControlToValidate="txtCity" Display="None" ErrorMessage="City Required"
                                                                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                <asp:ValidatorCalloutExtender
                                                                                    ID="ValidatorCalloutExtender_City" runat="server" CssClass="valiateField"
                                                                                    Enabled="True" TargetControlID="RequiredFieldValidator_City" PopupPosition="TopLeft">
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
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator_State" runat="server"
                                                                                    ControlToValidate="ddlState" Display="None" ErrorMessage="State Required"
                                                                                    SetFocusOnError="True">
                                                                                </asp:RequiredFieldValidator>
                                                                                <asp:ValidatorCalloutExtender
                                                                                    ID="ValidatorCalloutExtender_State" CssClass="valiateField"
                                                                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator_State" PopupPosition="TopLeft">
                                                                                </asp:ValidatorCalloutExtender>
                                                                                <label>State/Province</label>
                                                                                <asp:TextBox ID="ddlState" runat="server" CssClass="txtState validate"></asp:TextBox>

                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s5">
                                                                            <div class="row">
                                                                                <asp:TextBox ID="txtZip" MaxLength="10" runat="server"></asp:TextBox>
                                                                                <label for="txtZip">Zip</label>
                                                                            </div>
                                                                        </div>


                                                                        <div class="input-field col s2">
                                                                            <div class="row">
                                                                                &nbsp;
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s5">
                                                                            <div class="row">
                                                                                <label class="drpdwn-label">Country</label>
                                                                                <asp:CompareValidator
                                                                                    ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlCountry"
                                                                                    ErrorMessage="Country Required." Operator="NotEqual"
                                                                                    ValueToCompare="Select"
                                                                                    Display="None" SetFocusOnError="true" />
                                                                                <asp:ValidatorCalloutExtender
                                                                                    ID="ValidatorCalloutExtender3" CssClass="valiateField"
                                                                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator3" PopupPosition="TopLeft">
                                                                                </asp:ValidatorCalloutExtender>
                                                                                <asp:DropDownList ID="ddlCountry" runat="server" ToolTip="Country" CssClass="ddlCountry validate browser-default">
                                                                                    <asp:ListItem Value="Select">Select</asp:ListItem>
                                                                                    <asp:ListItem Value="US">United States</asp:ListItem>
                                                                                    <asp:ListItem Value="CA">Canada</asp:ListItem>

                                                                                    <asp:ListItem Value="AF">Afghanistan</asp:ListItem>

                                                                                    <asp:ListItem Value="AL">Albania</asp:ListItem>

                                                                                    <asp:ListItem Value="DZ">Algeria</asp:ListItem>

                                                                                    <asp:ListItem Value="AS">American Samoa</asp:ListItem>

                                                                                    <asp:ListItem Value="AD">Andorra</asp:ListItem>

                                                                                    <asp:ListItem Value="AO">Angola</asp:ListItem>

                                                                                    <asp:ListItem Value="AI">Anguilla</asp:ListItem>

                                                                                    <asp:ListItem Value="AQ">Antarctica</asp:ListItem>

                                                                                    <asp:ListItem Value="AG">Antigua And Barbuda</asp:ListItem>

                                                                                    <asp:ListItem Value="AR">Argentina</asp:ListItem>

                                                                                    <asp:ListItem Value="AM">Armenia</asp:ListItem>

                                                                                    <asp:ListItem Value="AW">Aruba</asp:ListItem>

                                                                                    <asp:ListItem Value="AU">Australia</asp:ListItem>

                                                                                    <asp:ListItem Value="AT">Austria</asp:ListItem>

                                                                                    <asp:ListItem Value="AZ">Azerbaijan</asp:ListItem>

                                                                                    <asp:ListItem Value="BS">Bahamas</asp:ListItem>

                                                                                    <asp:ListItem Value="BH">Bahrain</asp:ListItem>

                                                                                    <asp:ListItem Value="BD">Bangladesh</asp:ListItem>

                                                                                    <asp:ListItem Value="BB">Barbados</asp:ListItem>

                                                                                    <asp:ListItem Value="BY">Belarus</asp:ListItem>

                                                                                    <asp:ListItem Value="BE">Belgium</asp:ListItem>

                                                                                    <asp:ListItem Value="BZ">Belize</asp:ListItem>

                                                                                    <asp:ListItem Value="BJ">Benin</asp:ListItem>

                                                                                    <asp:ListItem Value="BM">Bermuda</asp:ListItem>

                                                                                    <asp:ListItem Value="BT">Bhutan</asp:ListItem>

                                                                                    <asp:ListItem Value="BO">Bolivia</asp:ListItem>

                                                                                    <asp:ListItem Value="BA">Bosnia And Herzegowina</asp:ListItem>

                                                                                    <asp:ListItem Value="BW">Botswana</asp:ListItem>

                                                                                    <asp:ListItem Value="BV">Bouvet Island</asp:ListItem>

                                                                                    <asp:ListItem Value="BR">Brazil</asp:ListItem>

                                                                                    <asp:ListItem Value="IO">British Indian Ocean Territory</asp:ListItem>

                                                                                    <asp:ListItem Value="BN">Brunei Darussalam</asp:ListItem>

                                                                                    <asp:ListItem Value="BG">Bulgaria</asp:ListItem>

                                                                                    <asp:ListItem Value="BF">Burkina Faso</asp:ListItem>

                                                                                    <asp:ListItem Value="BI">Burundi</asp:ListItem>

                                                                                    <asp:ListItem Value="KH">Cambodia</asp:ListItem>

                                                                                    <asp:ListItem Value="CM">Cameroon</asp:ListItem>


                                                                                    <asp:ListItem Value="CV">Cape Verde</asp:ListItem>

                                                                                    <asp:ListItem Value="KY">Cayman Islands</asp:ListItem>

                                                                                    <asp:ListItem Value="CF">Central African Republic</asp:ListItem>

                                                                                    <asp:ListItem Value="TD">Chad</asp:ListItem>

                                                                                    <asp:ListItem Value="CL">Chile</asp:ListItem>

                                                                                    <asp:ListItem Value="CN">China</asp:ListItem>

                                                                                    <asp:ListItem Value="CX">Christmas Island</asp:ListItem>

                                                                                    <asp:ListItem Value="CC">Cocos (Keeling) Islands</asp:ListItem>

                                                                                    <asp:ListItem Value="CO">Colombia</asp:ListItem>

                                                                                    <asp:ListItem Value="KM">Comoros</asp:ListItem>

                                                                                    <asp:ListItem Value="CG">Congo</asp:ListItem>

                                                                                    <asp:ListItem Value="CK">Cook Islands</asp:ListItem>

                                                                                    <asp:ListItem Value="CR">Costa Rica</asp:ListItem>

                                                                                    <asp:ListItem Value="CI">Cote D'Ivoire</asp:ListItem>

                                                                                    <asp:ListItem Value="HR">Croatia (Local Name: Hrvatska)</asp:ListItem>

                                                                                    <asp:ListItem Value="CU">Cuba</asp:ListItem>

                                                                                    <asp:ListItem Value="CY">Cyprus</asp:ListItem>

                                                                                    <asp:ListItem Value="CZ">Czech Republic</asp:ListItem>

                                                                                    <asp:ListItem Value="DK">Denmark</asp:ListItem>

                                                                                    <asp:ListItem Value="DJ">Djibouti</asp:ListItem>

                                                                                    <asp:ListItem Value="DM">Dominica</asp:ListItem>

                                                                                    <asp:ListItem Value="DO">Dominican Republic</asp:ListItem>

                                                                                    <asp:ListItem Value="TP">East Timor</asp:ListItem>

                                                                                    <asp:ListItem Value="EC">Ecuador</asp:ListItem>

                                                                                    <asp:ListItem Value="EG">Egypt</asp:ListItem>

                                                                                    <asp:ListItem Value="SV">El Salvador</asp:ListItem>

                                                                                    <asp:ListItem Value="GQ">Equatorial Guinea</asp:ListItem>

                                                                                    <asp:ListItem Value="ER">Eritrea</asp:ListItem>

                                                                                    <asp:ListItem Value="EE">Estonia</asp:ListItem>

                                                                                    <asp:ListItem Value="ET">Ethiopia</asp:ListItem>

                                                                                    <asp:ListItem Value="FK">Falkland Islands (Malvinas)</asp:ListItem>

                                                                                    <asp:ListItem Value="FO">Faroe Islands</asp:ListItem>

                                                                                    <asp:ListItem Value="FJ">Fiji</asp:ListItem>

                                                                                    <asp:ListItem Value="FI">Finland</asp:ListItem>

                                                                                    <asp:ListItem Value="FR">France</asp:ListItem>

                                                                                    <asp:ListItem Value="GF">French Guiana</asp:ListItem>

                                                                                    <asp:ListItem Value="PF">French Polynesia</asp:ListItem>

                                                                                    <asp:ListItem Value="TF">French Southern Territories</asp:ListItem>

                                                                                    <asp:ListItem Value="GA">Gabon</asp:ListItem>

                                                                                    <asp:ListItem Value="GM">Gambia</asp:ListItem>

                                                                                    <asp:ListItem Value="GE">Georgia</asp:ListItem>

                                                                                    <asp:ListItem Value="DE">Germany</asp:ListItem>

                                                                                    <asp:ListItem Value="GH">Ghana</asp:ListItem>

                                                                                    <asp:ListItem Value="GI">Gibraltar</asp:ListItem>

                                                                                    <asp:ListItem Value="GR">Greece</asp:ListItem>

                                                                                    <asp:ListItem Value="GL">Greenland</asp:ListItem>

                                                                                    <asp:ListItem Value="GD">Grenada</asp:ListItem>

                                                                                    <asp:ListItem Value="GP">Guadeloupe</asp:ListItem>

                                                                                    <asp:ListItem Value="GU">Guam</asp:ListItem>

                                                                                    <asp:ListItem Value="GT">Guatemala</asp:ListItem>

                                                                                    <asp:ListItem Value="GN">Guinea</asp:ListItem>

                                                                                    <asp:ListItem Value="GW">Guinea-Bissau</asp:ListItem>

                                                                                    <asp:ListItem Value="GY">Guyana</asp:ListItem>

                                                                                    <asp:ListItem Value="HT">Haiti</asp:ListItem>

                                                                                    <asp:ListItem Value="HM">Heard And Mc Donald Islands</asp:ListItem>

                                                                                    <asp:ListItem Value="VA">Holy See (Vatican City State)</asp:ListItem>

                                                                                    <asp:ListItem Value="HN">Honduras</asp:ListItem>

                                                                                    <asp:ListItem Value="HK">Hong Kong</asp:ListItem>

                                                                                    <asp:ListItem Value="HU">Hungary</asp:ListItem>

                                                                                    <asp:ListItem Value="IS">Icel And</asp:ListItem>

                                                                                    <asp:ListItem Value="IN">India</asp:ListItem>

                                                                                    <asp:ListItem Value="ID">Indonesia</asp:ListItem>

                                                                                    <asp:ListItem Value="IR">Iran (Islamic Republic Of)</asp:ListItem>

                                                                                    <asp:ListItem Value="IQ">Iraq</asp:ListItem>

                                                                                    <asp:ListItem Value="IE">Ireland</asp:ListItem>

                                                                                    <asp:ListItem Value="IL">Israel</asp:ListItem>

                                                                                    <asp:ListItem Value="IT">Italy</asp:ListItem>

                                                                                    <asp:ListItem Value="JM">Jamaica</asp:ListItem>

                                                                                    <asp:ListItem Value="JP">Japan</asp:ListItem>

                                                                                    <asp:ListItem Value="JO">Jordan</asp:ListItem>

                                                                                    <asp:ListItem Value="KZ">Kazakhstan</asp:ListItem>

                                                                                    <asp:ListItem Value="KE">Kenya</asp:ListItem>

                                                                                    <asp:ListItem Value="KI">Kiribati</asp:ListItem>

                                                                                    <asp:ListItem Value="KP">Korea, Dem People'S Republic</asp:ListItem>

                                                                                    <asp:ListItem Value="KR">Korea, Republic Of</asp:ListItem>

                                                                                    <asp:ListItem Value="KW">Kuwait</asp:ListItem>

                                                                                    <asp:ListItem Value="KG">Kyrgyzstan</asp:ListItem>

                                                                                    <asp:ListItem Value="LA">Lao People'S Dem Republic</asp:ListItem>

                                                                                    <asp:ListItem Value="LV">Latvia</asp:ListItem>

                                                                                    <asp:ListItem Value="LB">Lebanon</asp:ListItem>

                                                                                    <asp:ListItem Value="LS">Lesotho</asp:ListItem>

                                                                                    <asp:ListItem Value="LR">Liberia</asp:ListItem>

                                                                                    <asp:ListItem Value="LY">Libyan Arab Jamahiriya</asp:ListItem>

                                                                                    <asp:ListItem Value="LI">Liechtenstein</asp:ListItem>

                                                                                    <asp:ListItem Value="LT">Lithuania</asp:ListItem>

                                                                                    <asp:ListItem Value="LU">Luxembourg</asp:ListItem>

                                                                                    <asp:ListItem Value="MO">Macau</asp:ListItem>

                                                                                    <asp:ListItem Value="MK">Macedonia</asp:ListItem>

                                                                                    <asp:ListItem Value="MG">Madagascar</asp:ListItem>

                                                                                    <asp:ListItem Value="MW">Malawi</asp:ListItem>

                                                                                    <asp:ListItem Value="MY">Malaysia</asp:ListItem>

                                                                                    <asp:ListItem Value="MV">Maldives</asp:ListItem>

                                                                                    <asp:ListItem Value="ML">Mali</asp:ListItem>

                                                                                    <asp:ListItem Value="MT">Malta</asp:ListItem>

                                                                                    <asp:ListItem Value="MH">Marshall Islands</asp:ListItem>

                                                                                    <asp:ListItem Value="MQ">Martinique</asp:ListItem>

                                                                                    <asp:ListItem Value="MR">Mauritania</asp:ListItem>

                                                                                    <asp:ListItem Value="MU">Mauritius</asp:ListItem>

                                                                                    <asp:ListItem Value="YT">Mayotte</asp:ListItem>

                                                                                    <asp:ListItem Value="MX">Mexico</asp:ListItem>

                                                                                    <asp:ListItem Value="FM">Micronesia, Federated States</asp:ListItem>

                                                                                    <asp:ListItem Value="MD">Moldova, Republic Of</asp:ListItem>

                                                                                    <asp:ListItem Value="MC">Monaco</asp:ListItem>

                                                                                    <asp:ListItem Value="MN">Mongolia</asp:ListItem>

                                                                                    <asp:ListItem Value="MS">Montserrat</asp:ListItem>

                                                                                    <asp:ListItem Value="MA">Morocco</asp:ListItem>

                                                                                    <asp:ListItem Value="MZ">Mozambique</asp:ListItem>

                                                                                    <asp:ListItem Value="MM">Myanmar</asp:ListItem>

                                                                                    <asp:ListItem Value="NA">Namibia</asp:ListItem>

                                                                                    <asp:ListItem Value="NR">Nauru</asp:ListItem>

                                                                                    <asp:ListItem Value="NP">Nepal</asp:ListItem>

                                                                                    <asp:ListItem Value="NL">Netherlands</asp:ListItem>

                                                                                    <asp:ListItem Value="AN">Netherlands Ant Illes</asp:ListItem>

                                                                                    <asp:ListItem Value="NC">New Caledonia</asp:ListItem>

                                                                                    <asp:ListItem Value="NZ">New Zealand</asp:ListItem>

                                                                                    <asp:ListItem Value="NI">Nicaragua</asp:ListItem>

                                                                                    <asp:ListItem Value="NE">Niger</asp:ListItem>

                                                                                    <asp:ListItem Value="NG">Nigeria</asp:ListItem>

                                                                                    <asp:ListItem Value="NU">Niue</asp:ListItem>

                                                                                    <asp:ListItem Value="NF">Norfolk Island</asp:ListItem>

                                                                                    <asp:ListItem Value="MP">Northern Mariana Islands</asp:ListItem>

                                                                                    <asp:ListItem Value="NO">Norway</asp:ListItem>

                                                                                    <asp:ListItem Value="OM">Oman</asp:ListItem>

                                                                                    <asp:ListItem Value="PK">Pakistan</asp:ListItem>

                                                                                    <asp:ListItem Value="PW">Palau</asp:ListItem>

                                                                                    <asp:ListItem Value="PA">Panama</asp:ListItem>

                                                                                    <asp:ListItem Value="PG">Papua New Guinea</asp:ListItem>

                                                                                    <asp:ListItem Value="PY">Paraguay</asp:ListItem>

                                                                                    <asp:ListItem Value="PE">Peru</asp:ListItem>

                                                                                    <asp:ListItem Value="PH">Philippines</asp:ListItem>

                                                                                    <asp:ListItem Value="PN">Pitcairn</asp:ListItem>

                                                                                    <asp:ListItem Value="PL">Poland</asp:ListItem>

                                                                                    <asp:ListItem Value="PT">Portugal</asp:ListItem>

                                                                                    <asp:ListItem Value="PR">Puerto Rico</asp:ListItem>

                                                                                    <asp:ListItem Value="QA">Qatar</asp:ListItem>

                                                                                    <asp:ListItem Value="RE">Reunion</asp:ListItem>

                                                                                    <asp:ListItem Value="RO">Romania</asp:ListItem>

                                                                                    <asp:ListItem Value="RU">Russian Federation</asp:ListItem>

                                                                                    <asp:ListItem Value="RW">Rwanda</asp:ListItem>

                                                                                    <asp:ListItem Value="KN">Saint K Itts And Nevis</asp:ListItem>

                                                                                    <asp:ListItem Value="LC">Saint Lucia</asp:ListItem>

                                                                                    <asp:ListItem Value="VC">Saint Vincent, The Grenadines</asp:ListItem>

                                                                                    <asp:ListItem Value="WS">Samoa</asp:ListItem>

                                                                                    <asp:ListItem Value="SM">San Marino</asp:ListItem>

                                                                                    <asp:ListItem Value="ST">Sao Tome And Principe</asp:ListItem>

                                                                                    <asp:ListItem Value="SA">Saudi Arabia</asp:ListItem>

                                                                                    <asp:ListItem Value="SN">Senegal</asp:ListItem>

                                                                                    <asp:ListItem Value="SC">Seychelles</asp:ListItem>

                                                                                    <asp:ListItem Value="SL">Sierra Leone</asp:ListItem>

                                                                                    <asp:ListItem Value="SG">Singapore</asp:ListItem>

                                                                                    <asp:ListItem Value="SK">Slovakia (Slovak Republic)</asp:ListItem>

                                                                                    <asp:ListItem Value="SI">Slovenia</asp:ListItem>

                                                                                    <asp:ListItem Value="SB">Solomon Islands</asp:ListItem>

                                                                                    <asp:ListItem Value="SO">Somalia</asp:ListItem>

                                                                                    <asp:ListItem Value="ZA">South Africa</asp:ListItem>

                                                                                    <asp:ListItem Value="GS">South Georgia , S Sandwich Is.</asp:ListItem>

                                                                                    <asp:ListItem Value="ES">Spain</asp:ListItem>

                                                                                    <asp:ListItem Value="LK">Sri Lanka</asp:ListItem>

                                                                                    <asp:ListItem Value="SH">St. Helena</asp:ListItem>

                                                                                    <asp:ListItem Value="PM">St. Pierre And Miquelon</asp:ListItem>

                                                                                    <asp:ListItem Value="SD">Sudan</asp:ListItem>

                                                                                    <asp:ListItem Value="SR">Suriname</asp:ListItem>

                                                                                    <asp:ListItem Value="SJ">Svalbard, Jan Mayen Islands</asp:ListItem>

                                                                                    <asp:ListItem Value="SZ">Sw Aziland</asp:ListItem>

                                                                                    <asp:ListItem Value="SE">Sweden</asp:ListItem>

                                                                                    <asp:ListItem Value="CH">Switzerland</asp:ListItem>

                                                                                    <asp:ListItem Value="SY">Syrian Arab Republic</asp:ListItem>

                                                                                    <asp:ListItem Value="TW">Taiwan</asp:ListItem>

                                                                                    <asp:ListItem Value="TJ">Tajikistan</asp:ListItem>

                                                                                    <asp:ListItem Value="TZ">Tanzania, United Republic Of</asp:ListItem>

                                                                                    <asp:ListItem Value="TH">Thailand</asp:ListItem>

                                                                                    <asp:ListItem Value="TG">Togo</asp:ListItem>

                                                                                    <asp:ListItem Value="TK">Tokelau</asp:ListItem>

                                                                                    <asp:ListItem Value="TO">Tonga</asp:ListItem>

                                                                                    <asp:ListItem Value="TT">Trinidad And Tobago</asp:ListItem>

                                                                                    <asp:ListItem Value="TN">Tunisia</asp:ListItem>

                                                                                    <asp:ListItem Value="TR">Turkey</asp:ListItem>

                                                                                    <asp:ListItem Value="TM">Turkmenistan</asp:ListItem>

                                                                                    <asp:ListItem Value="TC">Turks And Caicos Islands</asp:ListItem>

                                                                                    <asp:ListItem Value="TV">Tuvalu</asp:ListItem>

                                                                                    <asp:ListItem Value="UG">Uganda</asp:ListItem>

                                                                                    <asp:ListItem Value="UA">Ukraine</asp:ListItem>

                                                                                    <asp:ListItem Value="AE">United Arab Emirates</asp:ListItem>

                                                                                    <asp:ListItem Value="GB">United Kingdom</asp:ListItem>



                                                                                    <asp:ListItem Value="UM">United States Minor Is.</asp:ListItem>

                                                                                    <asp:ListItem Value="UY">Uruguay</asp:ListItem>

                                                                                    <asp:ListItem Value="UZ">Uzbekistan</asp:ListItem>

                                                                                    <asp:ListItem Value="VU">Vanuatu</asp:ListItem>

                                                                                    <asp:ListItem Value="VE">Venezuela</asp:ListItem>

                                                                                    <asp:ListItem Value="VN">Viet Nam</asp:ListItem>

                                                                                    <asp:ListItem Value="VG">Virgin Islands (British)</asp:ListItem>

                                                                                    <asp:ListItem Value="VI">Virgin Islands (U.S.)</asp:ListItem>

                                                                                    <asp:ListItem Value="WF">Wallis And Futuna Islands</asp:ListItem>

                                                                                    <asp:ListItem Value="EH">Western Sahara</asp:ListItem>

                                                                                    <asp:ListItem Value="YE">Yemen</asp:ListItem>

                                                                                    <asp:ListItem Value="YU">Yugoslavia</asp:ListItem>

                                                                                    <asp:ListItem Value="ZR">Zaire</asp:ListItem>

                                                                                    <asp:ListItem Value="ZM">Zambia</asp:ListItem>

                                                                                    <asp:ListItem Value="ZW">Zimbabwe</asp:ListItem>

                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>

                                                                    </div>
                                                                    <div class="form-section3-blank">
                                                                        &nbsp;
                                                                    </div>
                                                                    <div class="form-section3">
                                                                        <div class="input-field col s5">
                                                                            <div class="row">
                                                                                <asp:TextBox ID="txtLatitude" runat="server" CssClass="validate"></asp:TextBox>
                                                                                <label for="txtLatitude">Latitude</label>
                                                                            </div>
                                                                        </div>


                                                                        <div class="input-field col s2">
                                                                            <div class="row">
                                                                                &nbsp;
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s5">
                                                                            <div class="row">
                                                                                <asp:TextBox ID="txtLongitude" runat="server" CssClass="validate"></asp:TextBox>
                                                                                <label for="txtLongitude">Longitude</label>
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <div id="map" style="overflow: hidden !important; height: 155px!important;"></div>
                                                                            </div>
                                                                        </div>


                                                                    </div>
                                                                </div>
                                                                <div class="form-section-row">
                                                                    <div class="section-ttle">Settings</div>
                                                                    <div class="form-section3">
                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <label for="txtUserName">Username<span class="reqd">*</span></label>

                                                                                <asp:TextBox ID="txtUserName" runat="server" CssClass="txtUserName form-control validate" MaxLength="50" onfocus="if (this.hasAttribute('readonly')) { this.removeAttribute('readonly');
                                                                                // fix for mobile safari to show virtual keyboard
                                                                                this.blur();    this.focus();  }"></asp:TextBox>
                                                                                <asp:FilteredTextBoxExtender ID="txtUserName_FilteredTextBoxExtender" runat="server"
                                                                                    Enabled="True" FilterMode="InvalidChars" InvalidChars="~*()+|}{&quot;:?&gt;&lt;/;'[]\`"
                                                                                    TargetControlID="txtUserName">
                                                                                </asp:FilteredTextBoxExtender>

                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtUserName"
                                                                                    Display="None" ErrorMessage="Username Required" SetFocusOnError="True">
                                                                                </asp:RequiredFieldValidator>
                                                                                <asp:ValidatorCalloutExtender
                                                                                    ID="RequiredFieldValidator4_ValidatorCalloutExtender" PopupPosition="TopLeft" runat="server" Enabled="True" CssClass="valiateField" TargetControlID="RequiredFieldValidator4">
                                                                                </asp:ValidatorCalloutExtender>

                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <label for="txtPassword">Password<span class="reqd">*</span></label>


                                                                                <asp:TextBox ID="txtPassword" runat="server" CssClass="txtPassword form-control validate" MaxLength="10" AutoCompleteType="Disabled" onfocus="if (this.hasAttribute('readonly')) { this.removeAttribute('readonly');
                                                                                // fix for mobile safari to show virtual keyboard
                                                                                this.blur();    this.focus();  }">

                                                                                </asp:TextBox>
                                                                                <asp:FilteredTextBoxExtender ID="txtPassword_FilteredTextBoxExtender" runat="server"
                                                                                    Enabled="False" FilterMode="InvalidChars" InvalidChars="~*()+|}{&quot;:?&gt;&lt;,/;'[]\`"
                                                                                    TargetControlID="txtPassword">
                                                                                </asp:FilteredTextBoxExtender>
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtPassword"
                                                                                    Display="None" ErrorMessage="Password Required" SetFocusOnError="True">
                                                                                </asp:RequiredFieldValidator>
                                                                                <asp:ValidatorCalloutExtender
                                                                                    ID="RequiredFieldValidator5_ValidatorCalloutExtender" PopupPosition="TopLeft" CssClass="valiateField" runat="server" Enabled="True" TargetControlID="RequiredFieldValidator5">
                                                                                </asp:ValidatorCalloutExtender>

                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-section3-blank">
                                                                        &nbsp;
                                                                    </div>
                                                                    <div class="form-section3">
                                                                        <div class="input-field col s5">
                                                                            <div class="row">
                                                                                <label class="drpdwn-label">Status</label>
                                                                                <asp:DropDownList ID="rbStatus" runat="server" CssClass="browser-default">
                                                                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                                                                    <asp:ListItem Value="1">Inactive</asp:ListItem>
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
                                                                                <label class="drpdwn-label">User Type</label>
                                                                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                                                    <ContentTemplate>
                                                                                        <asp:DropDownList ID="ddlUserType" runat="server" CssClass="browser-default"
                                                                                            OnSelectedIndexChanged="ddlUserType_SelectedIndexChanged" AutoPostBack="True">
                                                                                            <asp:ListItem Value="0">Office</asp:ListItem>
                                                                                            <asp:ListItem Value="1">Field</asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </div>
                                                                        </div>

                                                                        <div class="input-field col s5">
                                                                            <div class="row">
                                                                                <%--<input id="dateoT" type="text" class="datepicker_mom">--%>
                                                                                <asp:TextBox ID="txtStartDate" runat="server" autocomplete="off"
                                                                                    CssClass="datepicker_mom" onkeypress="return false;"></asp:TextBox>
                                                                                <label for="dateoT">Start Date</label>
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s2">
                                                                            <div class="row">
                                                                                &nbsp;
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s5">
                                                                            <div class="row">
                                                                                <%--<input id="dateoH" type="text" class="datepicker_mom">--%>
                                                                                <asp:TextBox ID="txtEndDate" runat="server" autocomplete="off"
                                                                                    CssClass="datepicker_mom" onkeypress="return false;"></asp:TextBox>
                                                                                <label for="dateoH">End Date</label>
                                                                            </div>
                                                                        </div>


                                                                    </div>
                                                                    <div class="form-section3-blank">
                                                                        &nbsp;
                                                                    </div>
                                                                    <div class="form-section3">

                                                                        <div class="input-field col s12">
                                                                            <div class="row Department-css" >
                                                                                <%--Multiselect Dropdown with Checkboxes, Use Telerik--%>
                                                                                <label class="drpdwn-label">Department</label>
                                                                                <telerik:RadComboBox ID="ddlDepartment" runat="server" Enabled="true" CheckBoxes="true"
                                                                                    EmptyMessage="Select Department" CssClass="browser-default col s12"
                                                                                    Sort="Ascending">
                                                                                </telerik:RadComboBox>
                                                                            </div>
                                                                        </div>



                                                                        <div class="input-field col s12 m-t-14" id="dvCompanyPermission" runat="server" style="margin-top: 14px;">
                                                                            <div class="row">
                                                                                <%--Multiselect Dropdown with Checkboxes, Use Telerik--%>
                                                                                <label class="drpdwn-label">Company</label>
                                                                                <telerik:RadComboBox ID="lstSelectCompany" runat="server" Enabled="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                                                                                    Localization-CheckAllString="Select all" EmptyMessage="Select Company" CssClass="browser-default col s12"
                                                                                    Sort="Ascending">
                                                                                </telerik:RadComboBox>
                                                                                <asp:Label ID="lblSelectedTech" runat="server" Visible="false"></asp:Label>
                                                                            </div>
                                                                        </div>

                                                                        <div class="input-field col s5 m-t-4">
                                                                            <div class="row">
                                                                                <label class="drpdwn-label">User Role</label>
                                                                                <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">UserRoleChange
                                                                                    <ContentTemplate>--%>
                                                                                <asp:DropDownList ID="ddlUserRole" runat="server" CssClass="browser-default"
                                                                                    onchange="javascript:return UserRoleChange(this);"
                                                                                    AutoPostBack="True" OnSelectedIndexChanged="ddlUserRole_SelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                                <%--</ContentTemplate>
                                                                                </asp:UpdatePanel>--%>
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s2">
                                                                            <div class="row">
                                                                                &nbsp;
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s5 m-t-4" id="divApplyUserRolePermission" runat="server">
                                                                            <div class="row">
                                                                                <label class="drpdwn-label">Applying User Role Permission</label>
                                                                                <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                                                    <ContentTemplate>--%>
                                                                                <asp:DropDownList ID="ddlApplyUserRolePermission" runat="server" CssClass="browser-default"
                                                                                    onchange="javascript:return ApplyUserRolePermissionChange(this);"
                                                                                    AutoPostBack="True" OnSelectedIndexChanged="ddlApplyUserRolePermission_SelectedIndexChanged">
                                                                                    <asp:ListItem Value="0">None</asp:ListItem>
                                                                                    <asp:ListItem Value="1">Merge</asp:ListItem>
                                                                                    <asp:ListItem Value="2">Override</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                                <%--</ContentTemplate>
                                                                                </asp:UpdatePanel>--%>
                                                                            </div>
                                                                        </div>

                                                                    </div>
                                                                </div>

                                                                <div class="form-section-row">
                                                                    <div class="section-ttle">Other Info</div>
                                                                    <div class="form-section3">
                                                                        <div class="input-field col s5">
                                                                            <div class="row">
                                                                                <%--    <input id="telephone" type="text" class="validate">--%>
                                                                                <asp:TextBox ID="txtTelephone" runat="server" CssClass="validate txtTelephone" MaxLength="22"
                                                                                    placeholder="(xxx)xxx-xxxx"></asp:TextBox>
                                                                                <label for="txtTelephone">Phone<span class="reqd">*</span></label>

                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server"
                                                                                    ControlToValidate="txtTelephone" Display="None" ErrorMessage="Telephone Required"
                                                                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                <asp:ValidatorCalloutExtender
                                                                                    ID="RequiredFieldValidator8_ValidatorCalloutExtender" runat="server"
                                                                                    Enabled="True" TargetControlID="RequiredFieldValidator8" PopupPosition="TopLeft" CssClass="valiateField">
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
                                                                                <%--<input id="cellular" type="text" class="">--%>
                                                                                <asp:TextBox ID="txtCell" runat="server" CssClass="validate" MaxLength="22"
                                                                                    placeholder="(xxx)xxx-xxxx"></asp:TextBox>
                                                                                <label for="txtCell">Cell</label>
                                                                            </div>
                                                                        </div>

                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <%--<input id="email" type="text" class="validate">--%>
                                                                                <%--<asp:TextBox ID="txtEmail" OnTextChanged="txtEmail_TextChanged" AutoPostBack="True" runat="server" CssClass="form-control" MaxLength="50"
                                                                                    TabIndex="10"></asp:TextBox>--%>
                                                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                                                    <ContentTemplate>
                                                                                        <asp:TextBox ID="txtEmail" OnTextChanged="txtEmail_TextChanged" AutoPostBack="true" runat="server" CssClass="form-control" MaxLength="50"
                                                                                            TabIndex="10"></asp:TextBox>

                                                                                        <asp:FilteredTextBoxExtender ID="txtEmail_FilteredTextBoxExtender" runat="server"
                                                                                            Enabled="True" FilterMode="InvalidChars" InvalidChars=" " TargetControlID="txtEmail">
                                                                                        </asp:FilteredTextBoxExtender>
                                                                                        <label for="email">Email</label>

                                                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                                                                            ControlToValidate="txtEmail" Display="None" ErrorMessage="Invalid Email" SetFocusOnError="True"
                                                                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                                                                        </asp:RegularExpressionValidator>
                                                                                        <asp:ValidatorCalloutExtender
                                                                                            ID="RegularExpressionValidator1_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                                            TargetControlID="RegularExpressionValidator1">
                                                                                        </asp:ValidatorCalloutExtender>
                                                                                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                                                                                            Display="None" ErrorMessage="Email Required" SetFocusOnError="True" Enabled="False">
                                                                                        </asp:RequiredFieldValidator>
                                                                                        <asp:ValidatorCalloutExtender ID="rfvEmail_ValidatorCalloutExtender"
                                                                                            runat="server" Enabled="True" TargetControlID="rfvEmail">
                                                                                        </asp:ValidatorCalloutExtender>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </div>
                                                                        </div>



                                                                        
                                                                        <div class="input-field col s12">
                                                                            <div class="row">

                                                                                <%--<asp:Label ID="lblMultiLang" runat="server" Text="Language"></asp:Label>--%>
                                                                                <label id="lblMultiLang" class="drpdwn-label" runat="server">Language</label>
                                                                                <asp:DropDownList ID="ddlLang" runat="server" CssClass="browser-default">
                                                                                    <asp:ListItem Value="select">-- Select --</asp:ListItem>
                                                                                    <asp:ListItem Value="english">English</asp:ListItem>
                                                                                    <asp:ListItem Value="spanish">Spanish</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>

                                                                    </div>
                                                                    <div class="form-section3-blank">
                                                                        &nbsp;
                                                                    </div>
                                                                    <div class="form-section3">
                                                                        <div class="input-field col s5">
                                                                            <div class="row">
                                                                                <label for="EmName">Emergency Contact Name</label>
                                                                                <%--<input type="text" id="EmName" />--%>
                                                                                <asp:TextBox ID="txtEmName" CssClass="validate" runat="server"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s2">
                                                                            <div class="row">
                                                                                &nbsp;
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s5">
                                                                            <div class="row">
                                                                                <label for="EmNum">Emergency Number</label>
                                                                                <%--<input type="text" id="EmNum" />--%>
                                                                                <asp:TextBox ID="txtEmNum" CssClass="validate" runat="server"></asp:TextBox>
                                                                            </div>
                                                                        </div>

                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <%--<input id="textmessage" type="text" class="validate">--%>
                                                                                <asp:TextBox ID="txtMsg" runat="server" CssClass="validate"></asp:TextBox>
                                                                                <label for="txtMsg">Text Message</label>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-section3-blank">
                                                                        &nbsp;
                                                                    </div>
                                                                    <div class="form-section3">


                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <%--<textarea id="remarks" class="materialize-textarea textarea-border smalltxtarea"></textarea>--%>
                                                                                <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine"
                                                                                    MaxLength="8000" CssClass="txtRemarks formmaterialize-textarea textarea-border smalltxtarea pnlAccounts"></asp:TextBox>
                                                                                <label for="txtRemarks" class="txtbrdlbl">Remarks</label>
                                                                            </div>
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

                                        <li id="tbSignature" runat="server">
                                            <div id="accrdSignature" class="collapsible-header accrd accordian-text-custom"><i class="mdi-file-cloud-circle"></i>Email</div>
                                            <div class="collapsible-body">
                                                <div class="form-content-wrap">
                                                    <div class="form-content-pd">
                                                        <div class="form-section-row">
                                                            <div class="col s12">
                                                                <div class="form-section-row">
                                                                    <div class="input-field col s12 mgntp10">
                                                                        <div class="checkrow">
                                                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                                                <ContentTemplate>
                                                                                    <asp:CheckBox ID="chkEmailAcc" runat="server" AutoPostBack="True"
                                                                                        OnCheckedChanged="chkEmailAcc_CheckedChanged" CssClass="filled-in" />
                                                                                    <asp:Label runat="server" Text="Email Account"></asp:Label>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <%--<asp:UpdatePanel runat="server" ID="updatepnlemail">
                                                                    <ContentTemplate>--%>
                                                                <telerik:RadAjaxPanel ID="RadAjaxPanel_EmailSigns" runat="server">
                                                                    <asp:Panel ID="pnlEmailAccount" runat="server" Visible="False">

                                                                        <div class="form-section-row">
                                                                            <div class="form-section3half">
                                                                                <div class="section-ttle">
                                                                                    Incoming Mail Settings
                                                                                </div>
                                                                                <div class="form-section-row">
                                                                                    <div class="input-field col s12">
                                                                                        <div class="row">
                                                                                            <label for="txtInServer">Incoming Mail Server (IMAP)</label>
                                                                                            <%--<input type="text" id="imap" />--%>
                                                                                            <asp:TextBox ID="txtInServer" runat="server"
                                                                                                MaxLength="100"></asp:TextBox>

                                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator19"
                                                                                                runat="server" ControlToValidate="txtInServer" Display="None" ErrorMessage="Required"
                                                                                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                            <asp:ValidatorCalloutExtender
                                                                                                ID="RequiredFieldValidator19_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                                                PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator19">
                                                                                            </asp:ValidatorCalloutExtender>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="input-field col s5 chkcss" style="border-bottom: 1px solid #9e9e9e">
                                                                                        <div class="checkrow">
                                                                                            <%--input id="ssl" type="checkbox" class="filled-in">--%>
                                                                                            <asp:CheckBox ID="chkSSL" CssClass="filled-in" runat="server" />
                                                                                            <label for="chkSSL" class="margin-top-3px">SSL</label>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="input-field col s2">
                                                                                        <div class="row">
                                                                                            &nbsp;
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="input-field col s5">
                                                                                        <div class="row">
                                                                                            <label for="portin">Port</label>
                                                                                            <%--<input type="text" id="portin" />--%>
                                                                                            <asp:TextBox ID="txtinPort" runat="server"></asp:TextBox>
                                                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                                                FilterMode="ValidChars" FilterType="Numbers" TargetControlID="txtinPort">
                                                                                            </asp:FilteredTextBoxExtender>

                                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ControlToValidate="txtinPort"
                                                                                                Display="None" ErrorMessage="Required" SetFocusOnError="True">
                                                                                            </asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                                                                ID="RequiredFieldValidator20_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                                                PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator20">
                                                                                            </asp:ValidatorCalloutExtender>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="input-field col s5">
                                                                                        <div class="row">
                                                                                            <label for="unamein">
                                                                                                Email Username
                                                                                            </label>
                                                                                            <%--            <input type="text" id="unamein" readonly onfocus="if (this.hasAttribute('readonly')) { this.removeAttribute('readonly');
                                                                                // fix for mobile safari to show virtual keyboard
                                                                                this.blur();    this.focus();  }" />--%>
                                                                                            <asp:TextBox ID="txtInUSername" runat="server" CssClass="validate" AutoCompleteType="Disabled" onfocus="if (this.hasAttribute('readonly')) { this.removeAttribute('readonly');
                                                                                            // fix for mobile safari to show virtual keyboard
                                                                                            this.blur();    this.focus();  }"
                                                                                                MaxLength="100"></asp:TextBox><%--onfocusout="CopyToBCC()"--%>
                                                                                            <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                                                                                ControlToValidate="txtInUSername" Display="None" ErrorMessage="Invalid Username"
                                                                                                SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                                                                            </asp:RegularExpressionValidator>
                                                                                            <asp:ValidatorCalloutExtender
                                                                                                ID="RegularExpressionValidator2_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                                                TargetControlID="RegularExpressionValidator2" PopupPosition="TopLeft">
                                                                                            </asp:ValidatorCalloutExtender>--%>
                                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ControlToValidate="txtInUSername"
                                                                                                Display="None" ErrorMessage="Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                            <asp:ValidatorCalloutExtender ID="RequiredFieldValidator21_ValidatorCalloutExtender"
                                                                                                runat="server" Enabled="True" PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator21">
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
                                                                                            <label for="pwdin">Email Password</label>
                                                                                            <%--   <input type="password" id="pwdin" readonly onfocus="if (this.hasAttribute('readonly')) { this.removeAttribute('readonly');
                                                                                // fix for mobile safari to show virtual keyboard
                                                                                this.blur();    this.focus();  }" />--%>
                                                                                            <asp:TextBox ID="txtInPassword" TextMode="Password" OnPreRender="txtInPassword_PreRender" runat="server" AutoCompleteType="Disabled" onfocus="if (this.hasAttribute('readonly')) { this.removeAttribute('readonly');
                                                                            // fix for mobile safari to show virtual keyboard
                                                                            this.blur();    this.focus();  }"
                                                                                                MaxLength="50"></asp:TextBox>
                                                                                            <%--<telerik:RadTextBox RenderMode="Auto" runat="server" ID="txtInPassword"  Width="200px" TextMode="Password"></telerik:RadTextBox>--%>

                                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server"
                                                                                                ControlToValidate="txtInPassword" Display="None" ErrorMessage="Required" SetFocusOnError="True">
                                                                                            </asp:RequiredFieldValidator>
                                                                                            <asp:ValidatorCalloutExtender
                                                                                                ID="RequiredFieldValidator22_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                                                PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator22">
                                                                                            </asp:ValidatorCalloutExtender>

                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="input-field col s5">
                                                                                        <div class="row">
                                                                                            <label for="pwdin">Bcc Email</label>
                                                                                            <asp:TextBox ID="txtBccEmail" runat="server" MaxLength="50" onfocus="if (this.hasAttribute('readonly')) { this.removeAttribute('readonly');
                                                                                            // fix for mobile safari to show virtual keyboard
                                                                                            this.blur();    this.focus();  }"></asp:TextBox>
                                                                                            <asp:RegularExpressionValidator
                                                                                                ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtBccEmail"
                                                                                                Display="None" ErrorMessage="Invalid Email" SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                                                                            </asp:RegularExpressionValidator>
                                                                                            <asp:ValidatorCalloutExtender
                                                                                                ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                                                                                                TargetControlID="RegularExpressionValidator4" PopupPosition="TopLeft">
                                                                                            </asp:ValidatorCalloutExtender>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="input-field col s12 mgntp17">
                                                                                        <div class="row">
                                                                                            <div class="btnlinks">
                                                                                                <%--<asp:LinkButton ID="btnTestIncoming" runat="server"  OnClick="btnTestIncoming_Click" Text="Test Settings" />--%>
                                                                                                <asp:LinkButton ID="btnTestIncoming" runat="server" OnClick="btnTestIncoming_Click" Text="Test Settings"></asp:LinkButton>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>

                                                                                </div>
                                                                            </div>
                                                                            <div class="form-section3half-blank">
                                                                                &nbsp;
                                                                            </div>
                                                                            <div class="form-section3half">

                                                                                <div class="section-ttle">
                                                                                    Outgoing Mail Settings
                                                 
                                                                                </div>
                                                                                <div class="form-section-row">
                                                                                    <div class="input-field col s12">
                                                                                        <div class="row">
                                                                                            <label for="txtOutServer">Outgoing Mail Server (SMTP)</label>
                                                                                            <%--<input type="text" id="smtp" />--%>
                                                                                            <asp:TextBox ID="txtOutServer" runat="server" MaxLength="100"></asp:TextBox>
                                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator23"
                                                                                                runat="server" ControlToValidate="txtOutServer" Display="None" ErrorMessage="Required"
                                                                                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                            <asp:ValidatorCalloutExtender
                                                                                                ID="RequiredFieldValidator23_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                                                PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator23">
                                                                                            </asp:ValidatorCalloutExtender>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="input-field col s5 chkcss" style="border-bottom: 1px solid #9e9e9e">
                                                                                        <div class="checkrow">
                                                                                            <%--<input id="sslo" type="checkbox" class="filled-in">--%>
                                                                                            <asp:CheckBox ID="chkSame" runat="server" CssClass="filled-in" AutoPostBack="True" OnCheckedChanged="chkSame_CheckedChanged" />
                                                                                            <label for="chkSame" class="margin-top-3px">Same as Incoming</label>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="input-field col s2">
                                                                                        <div class="row">
                                                                                            &nbsp;
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="input-field col s5">
                                                                                        <div class="row">
                                                                                            <label for="porto">Port</label>
                                                                                            <%--<input type="text" id="porto" />--%>
                                                                                            <asp:TextBox ID="txtOutPort" runat="server"></asp:TextBox>
                                                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                                                FilterMode="ValidChars" FilterType="Numbers" TargetControlID="txtOutPort">
                                                                                            </asp:FilteredTextBoxExtender>
                                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ControlToValidate="txtOutPort"
                                                                                                Display="None" ErrorMessage="Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                            <asp:ValidatorCalloutExtender
                                                                                                ID="RequiredFieldValidator24_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                                                PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator24">
                                                                                            </asp:ValidatorCalloutExtender>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="input-field col s5">
                                                                                        <div class="row">
                                                                                            <label for="unameo">Email Username</label>
                                                                                            <%--<input type="text" id="unameo" readonly onfocus="if (this.hasAttribute('readonly')) { this.removeAttribute('readonly');
                                                                                            // fix for mobile safari to show virtual keyboard
                                                                                            this.blur();    this.focus();  }" />--%>
                                                                                            <asp:TextBox ID="txtOutUsername" runat="server" MaxLength="100">
                                                                                            </asp:TextBox>
                                                                                            <%--onfocus="if (this.hasAttribute('readonly')) 
                                                                                                        { 
                                                                                                            this.removeAttribute('readonly');
                                                                                                            // fix for mobile safari to show virtual keyboard
                                                                                                            this.blur();
                                                                                                            this.focus(); 
                                                                                                        }"--%>
                                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server"
                                                                                                ControlToValidate="txtOutUsername" Display="None" ErrorMessage="Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                            <%--<asp:RegularExpressionValidator
                                                                                                ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtOutUsername"
                                                                                                Display="None" ErrorMessage="Invalid Username" SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                                                                            </asp:RegularExpressionValidator>
                                                                                            <asp:ValidatorCalloutExtender
                                                                                                ID="RegularExpressionValidator3_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                                                TargetControlID="RegularExpressionValidator3" PopupPosition="TopLeft">
                                                                                            </asp:ValidatorCalloutExtender>--%>
                                                                                            <asp:ValidatorCalloutExtender ID="RequiredFieldValidator25_ValidatorCalloutExtender"
                                                                                                runat="server" Enabled="True" PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator25">
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
                                                                                            <label for="pwdin">Email Password</label>
                                                                                            <%--<input type="password" id="pwdo" readonly onfocus="if (this.hasAttribute('readonly')) { this.removeAttribute('readonly');
                                                                                            // fix for mobile safari to show virtual keyboard
                                                                                            this.blur();    this.focus();  }" />--%>
                                                                                            <asp:TextBox ID="txtOutPassword" TextMode="Password" OnPreRender="txtOutPassword_PreRender" runat="server" MaxLength="50">
                                                                                            </asp:TextBox>
                                                                                            <!--onfocus="if (this.hasAttribute('readonly')) 
                                                                                                            { 
                                                                                                                this.removeAttribute('readonly');
                                                                                                                // fix for mobile safari to show virtual keyboard
                                                                                                                this.blur();
                                                                                                                this.focus();  
                                                                                                            }"-->
                                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server"
                                                                                                ControlToValidate="txtOutPassword" Display="None" ErrorMessage="Required" SetFocusOnError="True">
                                                                                            </asp:RequiredFieldValidator>
                                                                                            <asp:ValidatorCalloutExtender
                                                                                                ID="RequiredFieldValidator26_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                                                PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator26">
                                                                                            </asp:ValidatorCalloutExtender>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="input-field col s5" style="border-bottom: 1px solid #9e9e9e">
                                                                                        <div class="checkrow">
                                                                                            <%--<input id="sslo" type="checkbox" class="filled-in">--%>
                                                                                            <asp:CheckBox ID="chkTakeASentEmailCopy" runat="server" CssClass="filled-in" />
                                                                                            <label for="chkTakeASentEmailCopy" class="margin-top-3px">Send Copy to "Sent Items"</label>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="input-field col s12 mgntp17">
                                                                                        <div class="row">
                                                                                            <div class="btnlinks">
                                                                                                <asp:LinkButton ID="btnTestOut" runat="server" OnClick="btnTestOut_Click" Text="Test Settings" />
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>

                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        
                                                                    </asp:Panel>
                                                                </telerik:RadAjaxPanel>
                                                                <asp:Panel ID="pnlEmailSignature" runat="server" Visible="False">
                                                                            <div class="form-section-row">
                                                                                <div class="section-ttle">Email signature</div>
                                                                                <div class="btncontainer">
                                                                                    <div class="btnlinks">
                                                                                        <asp:LinkButton ID="lnkAddEmailSign" runat="server" CausesValidation="False" OnClientClick="OpenAddEmailSignatureWindow();return false;">Add</asp:LinkButton>
                                                                                    </div>
                                                                                    <div class="btnlinks">
                                                                                        <asp:LinkButton ID="btnEditEmailSign" runat="server" CausesValidation="False" OnClientClick="OpenEditEmailSignatureWindow();return false;">Edit</asp:LinkButton>
                                                                                    </div>
                                                                                    <div class="btnlinks">
                                                                                        <asp:LinkButton ID="btnDeleteEmailSign" runat="server" OnClick="btnDeleteEmailSign_Click">Delete</asp:LinkButton>
                                                                                    </div>
                                                                                    <div style="display: none;">
                                                                                        <a id="lnkRefreshScreen" style="display: none;" onclick="RefreshScreen()"></a>
                                                                                        <asp:LinkButton ID="btnRefreshScreen" Style="display: none;" runat="server" CausesValidation="false" OnClick="lnkRefreshScreen_Click"></asp:LinkButton>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="grid_container m-t-10">
                                                                                    <div class="RadGrid RadGrid_Material FormGrid">
                                                                                        <div class="form-section-row m-b-0" style="margin-bottom: 0 !important;">
                                                                                            <div class="RadGrid RadGrid_Material">
                                                                                                <%--<telerik:RadAjaxPanel ID="RadAjaxPanel_EmailSigns" runat="server">--%>
                                                                                                    <telerik:RadGrid RenderMode="Auto" ID="RadGrid_EmailSigns" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                                                                        OnNeedDataSource="RadGrid_EmailSigns_NeedDataSource" PagerStyle-AlwaysVisible="true"
                                                                                                        ShowStatusBar="true" runat="server" AllowPaging="false" AllowSorting="true" Width="100%" AllowCustomPaging="True">
                                                                                                        <CommandItemStyle />
                                                                                                        <GroupingSettings CaseSensitive="false" />
                                                                                                        <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                                                            <Selecting AllowRowSelect="True"></Selecting>
                                                                                                            <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                                                            <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                                                        </ClientSettings>
                                                                                                        <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowFooter="true">
                                                                                                            <Columns>
                                                                                                                <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderStyle-Width="40">
                                                                                                                </telerik:GridClientSelectColumn>

                                                                                                                <telerik:GridTemplateColumn DataField="SignName" HeaderText="Name" SortExpression="SignName"
                                                                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="200"
                                                                                                                    ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblSignName" Text='<%# Eval("SignName")%>' runat="server"></asp:Label>
                                                                                                                        <asp:HiddenField ID="hdnSignId" Value='<%# Bind("Id") %>' runat="server" />
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn DataField="SignContent" HeaderText="Signature" HeaderStyle-Width="400"
                                                                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Title"
                                                                                                                    ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblSignContent" Text='<%# Eval("SignContent")%>' runat="server"></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>

                                                                                                                <telerik:GridTemplateColumn DataField="IsDefault" HeaderText="Is Default" HeaderStyle-Width="100"
                                                                                                                    AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" SortExpression="Title"
                                                                                                                    ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblIsDefault" Text='<%# (Convert.ToString(Eval("IsDefault")) == "True") ? "Yes" : "No" %>' runat="server"></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                            </Columns>
                                                                                                        </MasterTableView>
                                                                                                    </telerik:RadGrid>
                                                                                                <%--</telerik:RadAjaxPanel>--%>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="cf"></div>
                                                                            </div>
                                                                        </asp:Panel>
                                                                   <%-- </ContentTemplate>
                                                                </asp:UpdatePanel>--%>
                                                            </div>
                                                        </div>
                                                        <div class="cf"></div>
                                                        
                                                    </div>
                                                </div>
                                                <div style="clear: both;"></div>
                                            </div>
                                        </li>

                                        <li>
                                            <div id="accrdCred" class="collapsible-header accrd accordian-text-custom"><i class="mdi-social-people"></i>Field Worker Options</div>
                                            <div class="collapsible-body">
                                                <div class="form-content-wrap form-content-wrapwd">
                                                    <div class="form-content-pd">
                                                        <div class="form-section-row">
                                                            <div class="section-ttle">Options</div>
                                                            <asp:UpdatePanel ID="UpdatePanel21" runat="server">
                                                                <%--  <telerik:RadAjaxPanel  ID="rapWorker"  runat="server">--%>
                                                                <ContentTemplate>
                                                                    <asp:Panel ID="pnlWorker" runat="server" Enabled="False">

                                                                        <div class="form-section3">
                                                                            <div class="input-field col s4">
                                                                                <div class="checkrow">
                                                                                    <%--<input id="scdlboard" type="checkbox" class="filled-in">--%>
                                                                                    <asp:CheckBox ID="chkScheduleBrd" runat="server" CssClass="filled-in"/>
                                                                                    <label for="lblSchbrd"  style="margin-top:14px!important;" >Schedule Board</label>
                                                                                </div>
                                                                            </div>

                                                                            <div class="input-field col s5 ">
                                                                                <div class="checkrow">
                                                                                    <%--<input id="athDevice" type="checkbox" class="filled-in">--%>
                                                                                    <asp:CheckBox ID="chkMSAuthorisedDeviceOnly" runat="server" OnCheckedChanged="chkMSAuthorisedDeviceOnly_CheckedChanged" AutoPostBack="true" CssClass="chkMSAuthorisedDeviceOnly filled-in" />
                                                                                    <label for="chkMSAuthorisedDeviceOnly"  style="margin-top:14px!important;" >Authorized Device</label>
                                                                                </div>
                                                                            </div>
                                                                            <div class="input-field col s3 ">
                                                                                <div class="checkrow">
                                                                                    <%--<input id="maps" type="checkbox" class="filled-in">--%>
                                                                                    <asp:CheckBox ID="chkMap" CssClass="filled-in" runat="server" AutoPostBack="True"
                                                                                        OnCheckedChanged="chkMap_CheckedChanged"/>
                                                                                    <%--<asp:Label ID="lblMap" runat="server" Text="Maps"></asp:Label>--%>
                                                                                    <label id="lblMap" runat="server"  style="margin-top:14px!important;" >Maps</label>
                                                                                </div>
                                                                            </div>
                                                                            <div class="input-field col s11 m-t-5" >
                                                                                <div class="row">
                                                                                    <label class="drpdwn-label">Merchant ID</label>
                                                                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                                                        <ContentTemplate>
                                                                                            <asp:DropDownList ID="ddlMerchantID" runat="server" onchange="AddNewMerchant(this);"
                                                                                                CssClass="browser-default" Style="float: left;" TabIndex="12">
                                                                                            </asp:DropDownList>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </div>
                                                                            </div>
                                                                            <div class="input-field col s1 m-t-5">
                                                                                <div class="row">
                                                                                    <div class="btnlinksicon caus-css" >
                                                                                        <%--<a href="#"><i class="mdi-editor-border-color"></i></a>--%>
                                                                                        <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                                                                            <ContentTemplate>
                                                                                                <asp:LinkButton ID="imgbtnMerchant" runat="server" CausesValidation="False"
                                                                                                    OnClick="imgbtnMerchant_Click" ToolTip="Edit" Height="30px">
                                                                                        <i class="mdi-editor-border-color"></i>
                                                                                                </asp:LinkButton>
                                                                                            </ContentTemplate>
                                                                                        </asp:UpdatePanel>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-section3-blank">
                                                                            &nbsp;
                                                                        </div>

                                                                        <div class="form-section3">


                                                                            <div class="input-field col s12">
                                                                                <div class="row">
                                                                                    <%--<input id="devID" type="text">--%>
                                                                                    <%--<asp:Label ID="lblDeviceID" runat="server" Text="Device ID"></asp:Label>--%>
                                                                                    <label id="lblDeviceID" runat="server">Device ID</label>
                                                                                    <asp:TextBox ID="txtDeviceID" CssClass="txtDeviceID" runat="server" MaxLength="50"></asp:TextBox>
                                                                                    <%--   <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtDeviceID"
                                                                                            Display="None" Enabled="False" ErrorMessage="Device ID Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                        <asp:ValidatorCalloutExtender ID="RequiredFieldValidator11_ValidatorCalloutExtender"
                                                                                            runat="server" Enabled="True" PopupPosition="Right" TargetControlID="RequiredFieldValidator11">
                                                                                        </asp:ValidatorCalloutExtender>--%>
                                                                                </div>
                                                                            </div>

                                                                            <div class="input-field col s12">
                                                                                <div class="row">
                                                                                    <%--<input id="authdevID" type="text">--%>
                                                                                    <asp:TextBox ID="txtAuthdevID" CssClass="txtAuthdevID" runat="server"></asp:TextBox>
                                                                                    <label for="authdevID">Authorized Device ID</label>
                                                                                    <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server"
                                                                                        ControlToValidate="txtAuthdevID"  Enabled="false" Display="None" ErrorMessage="Authorized Device ID Required"
                                                                                        SetFocusOnError="True">

                                                                                    </asp:RequiredFieldValidator>
                                                                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator12_ValidatorCalloutExtender"
                                                                                        runat="server" CssClass="valiateField" PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator12">
                                                                                    </asp:ValidatorCalloutExtender>--%>
                                                                                </div>
                                                                            </div>

                                                                        </div>
                                                                        <div class="form-section3-blank">
                                                                            &nbsp;
                                                                        </div>
                                                                        <div class="form-section3">
                                                                            <div class="input-field col s12">
                                                                                <div class="row">
                                                                                    <asp:Label ID="lblSuper" runat="server" CssClass="drpdwn-label" Text="Supervisor"></asp:Label>
                                                                                    <%--<label id="lblSuper"  runat="server" class="drpdwn-label">Supervisor</label>--%>
                                                                                    <asp:DropDownList ID="ddlSuper" runat="server" AutoPostBack="True" CssClass="ddlSuper browser-default"
                                                                                        OnSelectedIndexChanged="ddlSuper_SelectedIndexChanged">
                                                                                    </asp:DropDownList>
                                                                                </div>
                                                                            </div>
                                                                            <div class="input-field col s6">
                                                                                <div class="checkrow">
                                                                                    <asp:CheckBox ID="chkSuper" runat="server" AutoPostBack="True" OnCheckedChanged="chkSuper_CheckedChanged"
                                                                                        CssClass="filled-in" style="margin-top: -4px!important;" />
                                                                                    <asp:Label ID="lblIsSuper" runat="server" Text="Is Supervisor"></asp:Label>
                                                                                </div>
                                                                            </div>
                                                                            <div class="input-field col s6">
                                                                                <div class="checkrow">
                                                                                    <asp:CheckBox ID="chkDefaultWorker" runat="server" CssClass="filled-in" style="margin-top: -4px!important;"/>
                                                                                    <asp:Label ID="lblDefwork" runat="server" Text="Default Worker"></asp:Label>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </asp:Panel>
                                                                </ContentTemplate>
                                                                <%--   </telerik:RadAjaxPanel>--%>
                                                            </asp:UpdatePanel>
                                                        </div>

                                                        <div class="form-section-row" id="userlist">
                                                            <div class="section-ttle">
                                                                Workers
                                                            </div>
                                                            <%--<telerik:RadAjaxLoadingPanel runat="server">--%>
                                                            <div class="btnCustomlinks">
                                                                <asp:LinkButton ID="btnEdit" runat="server" CssClass="btnCustomEdit" OnClick="btnEdit_Click" CausesValidation="False" ToolTip="Edit">Edit</asp:LinkButton>

                                                                <%-- <telerik:RadButton ID="btnEdit" runat="server" CssClass="btnEdit" OnClick="btnEdit_Click" CausesValidation="False" ToolTip="Edit" Text="Edit"></telerik:RadButton>--%>
                                                            </div>
                                                            <div class="btnlinks">
                                                                <asp:LinkButton ID="lnkDone" runat="server" CssClass="lnkDone" OnClick="lnkDone_Click" CausesValidation="False">Done</asp:LinkButton>
                                                            </div>
                                                            <%-- </telerik:RadAjaxLoadingPanel>--%>
                                                            <div class="RadGrid RadGrid_Material">
                                                                <telerik:RadCodeBlock ID="codeBlock1" runat="server">
                                                                    <script type="text/javascript">
                                                                        function pageLoad() {
                                                                            var grid = $find("<%= gvUsers.ClientID %>");
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
                                                                <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_gvUsers" runat="server">
                                                                </telerik:RadAjaxLoadingPanel>
                                                                <telerik:RadAjaxPanel ID="UpdatePanel8" runat="server" LoadingPanelID="RadAjaxLoadingPanel_gvUsers" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                                    <asp:HiddenField ID="hfCheckEdit" runat="server" Value="false" />
                                                                    <div class="grid_container mgntp10" id="pnlGrid" runat="server">
                                                                        <telerik:RadGrid ID="gvUsers" RenderMode="Auto" runat="server" AutoGenerateColumns="False" CssClass=" gvUsers table table-bordered table-striped table-condensed flip-content"
                                                                            AllowSorting="True" AllowPaging="true" OnSorting="gvUsers_Sorting"
                                                                            PageSize="20"
                                                                            ShowFooter="True"
                                                                            FilterType="CheckList"
                                                                            OnNeedDataSource="gvUsers_NeedDataSource"
                                                                            OnItemDataBound="gvUsers_ItemDataBound"
                                                                            OnPageIndexChanged="gvUsers_PageIndexChanged"
                                                                            OnPageSizeChanged="gvUsers_PageSizeChanged">
                                                                            <PagerStyle AlwaysVisible="true" Mode="NextPrevAndNumeric" />
                                                                            <CommandItemStyle />
                                                                            <GroupingSettings CaseSensitive="false" />
                                                                            <ActiveItemStyle CssClass="evenrowcolor" />
                                                                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                                <Selecting AllowRowSelect="True"></Selecting>
                                                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                            </ClientSettings>
                                                                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True" DataKeyNames="UserID">
                                                                                <Columns>
                                                                                    <telerik:GridTemplateColumn Visible="false" AllowFiltering="false">
                                                                                        <ItemTemplate>
                                                                                            <asp:HiddenField ID="hdnSelected" runat="server" />
                                                                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn HeaderText="ID" Visible="False" UniqueName="UserID" DataField="UserID" AllowFiltering="false">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblId" runat="server" Text='<%# Bind("UserID") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn HeaderText="TypeID" Visible="false" UniqueName="usertypeid" DataField="usertypeid">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblTypeid" runat="server" Text='<%# Bind("usertypeid") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn HeaderText="Username" SortExpression="fuser" UniqueName="fuser" DataField="fuser" ShowFilterIcon="false" FilterDelay="5" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("fuser") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn HeaderText="First Name" SortExpression="ffirst" UniqueName="ffirst" DataField="ffirst" ShowFilterIcon="false" FilterDelay="5" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="Label3" runat="server"><%#Eval("ffirst")%></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn HeaderText="Last Name" SortExpression="last" UniqueName="last" DataField="last" ShowFilterIcon="false" FilterDelay="5" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblLN" runat="server"><%#Eval("last")%></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn HeaderText="Type" SortExpression="usertype" UniqueName="usertype" DataField="usertype" ShowFilterIcon="false" FilterDelay="5" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblActive" runat="server"><%#Eval("usertype")%></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn HeaderText="Status" SortExpression="status" UniqueName="status" DataField="status" ShowFilterIcon="false" FilterDelay="5" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblStatus" runat="server"><%# Convert.ToInt32( Eval("status")) == 0 ? "Active" : "Inactive"%></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn HeaderText="Supervisor" SortExpression="super" UniqueName="super" DataField="super" ShowFilterIcon="false" FilterDelay="5" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblsuper" runat="server"><%#Eval("super")%></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                </Columns>
                                                                            </MasterTableView>
                                                                            <SelectedItemStyle CssClass="selectedrowcolor" />
                                                                            <AlternatingItemStyle CssClass="oddrowcolor" />
                                                                            <FilterMenu CssClass="RadFilterMenu_CheckList">
                                                                            </FilterMenu>
                                                                        </telerik:RadGrid>
                                                                    </div>
                                                                </telerik:RadAjaxPanel>
                                                            </div>
                                                            <div class="form-section-row">
                                                                <asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Style="display: none"
                                                                    CausesValidation="False" />


                                                                <asp:ModalPopupExtender runat="server" ID="programmaticModalPopup" BehaviorID="programmaticModalPopupBehavior"
                                                                    TargetControlID="hiddenTargetControlForModalPopup" PopupControlID="pnlMerchant"
                                                                    BackgroundCssClass="ModalPopupBG" Enabled="True">
                                                                </asp:ModalPopupExtender>



                                                            </div>
                                                            <div class="form-section-row">
                                                                <asp:Panel runat="server" ID="pnlMerchant" CssClass="table-merchant" Style="background-color: #fff; border: 1px solid #316b9d; display: none;">
                                                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Conditional">
                                                                        <Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="ddlMerchantID" />
                                                                            <asp:AsyncPostBackTrigger ControlID="imgbtnMerchant" />
                                                                        </Triggers>
                                                                        <ContentTemplate>
                                                                            <div id="divInner">
                                                                                <div runat="server" id="titleBar" class="model-popup-body">
                                                                                    <asp:Label CssClass="title_text" ID="Label1" Style="color: white" runat="server">Merchant</asp:Label>
                                                                                    <asp:LinkButton ID="lnkCancelMerchant" runat="server" CausesValidation="False" Style="float: right; color: #fff; margin-left: 10px; height: 16px;"
                                                                                        OnClick="lnkCancelMerchant_Click">Close</asp:LinkButton>
                                                                                    <asp:LinkButton ID="lnkSaveMerchant" runat="server" Height="16px" Style="float: right; color: #fff; margin-left: 10px;"
                                                                                        ValidationGroup="mrt" OnClick="lnkSaveMerchant_Click">Save</asp:LinkButton>
                                                                                    <asp:LinkButton ID="imgbtnDelete" Visible="false" runat="server" ImageUrl="images/delete.png" CausesValidation="false"
                                                                                        ToolTip="Delete Merchant" Style="width: 40px; float: right; color: #fff; margin-right: 15px; height: 20px; width: 20px;"
                                                                                        OnClick="imgbtnDelete_Click" OnClientClick="return confirm('Are you sure you want to delete the Merchant?')">Delete</asp:LinkButton>
                                                                                </div>
                                                                                <div style="padding: 15px">
                                                                                    <table style="height: 200px; padding: 10px">
                                                                                        <tr>
                                                                                            <td>Merchant ID
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txtMerchantID"
                                        Display="None" ErrorMessage="Merchant ID Required" SetFocusOnError="True" ValidationGroup="mrt"></asp:RequiredFieldValidator>
                                                                                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator13_ValidatorCalloutExtender"
                                                                                                    runat="server" Enabled="True" PopupPosition="BottomRight" TargetControlID="RequiredFieldValidator13">
                                                                                                </asp:ValidatorCalloutExtender>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtMerchantID" runat="server" class="form-control" MaxLength="100"></asp:TextBox>
                                                                                                <asp:HiddenField ID="hdnMerchantInfoID" runat="server" Value="0" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>Login ID
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="txtLoginID"
                                        Display="None" ErrorMessage="Login ID Required" SetFocusOnError="True" ValidationGroup="mrt"></asp:RequiredFieldValidator>
                                                                                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator14_ValidatorCalloutExtender"
                                                                                                    runat="server" Enabled="True" PopupPosition="TopRight" TargetControlID="RequiredFieldValidator14">
                                                                                                </asp:ValidatorCalloutExtender>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtLoginID" runat="server" class="form-control" MaxLength="100"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>Username<asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server"
                                                                                                ControlToValidate="txtMerchantUsername" Display="None" ErrorMessage="Username Required"
                                                                                                SetFocusOnError="True" ValidationGroup="mrt"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                                                                    ID="RequiredFieldValidator15_ValidatorCalloutExtender" PopupPosition="TopLeft" runat="server" Enabled="True"
                                                                                                    TargetControlID="RequiredFieldValidator15">
                                                                                                </asp:ValidatorCalloutExtender>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtMerchantUsername" runat="server" class="form-control"
                                                                                                    MaxLength="20"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>Password<asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server"
                                                                                                ControlToValidate="txtMerchantPassword" Display="None" ErrorMessage="Password Required"
                                                                                                SetFocusOnError="True" ValidationGroup="mrt"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                                                                    ID="RequiredFieldValidator16_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                                                    TargetControlID="RequiredFieldValidator16" PopupPosition="TopLeft">
                                                                                                </asp:ValidatorCalloutExtender>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtMerchantPassword" runat="server" class="form-control"
                                                                                                    MaxLength="100"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </asp:Panel>
                                                            </div>
                                                            <div class="cf"></div>
                                                        </div>
                                                    </div>
                                                    <div style="clear: both;"></div>
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
                                                                User Permissions
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
                                                                                <asp:CheckBox ID="chkCustomerModule" CssClass="css-checkbox" AutoPostBack="true" OnCheckedChanged="chkCustomerModule_CheckedChanged" runat="server" Font-Bold="true" Text="Customer module" />
                                                                            </div>
                                                                            <div class="input-field col s12">
                                                                                <div class="row">
                                                                                    <table>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">Customer </td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkCustomeradd" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                                <asp:CheckBox ID="chkCustomeredit" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                                <asp:CheckBox ID="chkCustomerdelete" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                                <asp:CheckBox ID="chkCustomerview" OnCheckedChanged="chkCustomerview_CheckedChanged" AutoPostBack="true" CssClass="css-checkbox" runat="server" Text="View" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">Location
                                                                                            </td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkLocationadd" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                                <asp:CheckBox ID="chkLocationedit" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                                <asp:CheckBox ID="chkLocationdelete" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                                <asp:CheckBox ID="chkLocationview" OnCheckedChanged="chkLocationview_CheckedChanged" AutoPostBack="true" CssClass="css-checkbox" runat="server" Text="View" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">Equipment
                                                                                            </td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkEquipmentsadd" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                                <asp:CheckBox ID="chkEquipmentsedit" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                                <asp:CheckBox ID="chkEquipmentsdelete" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                                <asp:CheckBox ID="chkEquipmentsview" OnCheckedChanged="chkEquipmentsview_CheckedChanged" AutoPostBack="true" CssClass="css-checkbox" runat="server" Text="View" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">Receive Payment</td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkReceivePaymentAdd" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                                <asp:CheckBox ID="chkReceivePaymentEdit" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                                <asp:CheckBox ID="chkReceivePaymentDelete" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                                <asp:CheckBox ID="chkReceivePaymentView" OnCheckedChanged="chkReceivePaymentView_CheckedChanged" AutoPostBack="true" CssClass="css-checkbox" runat="server" Text="View" />
                                                                                            </td>
                                                                                        </tr>
<%--                                                                                        <tr runat="server">
                                                                                            <td runat="server">Online Payment</td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkOnlinePaymentAdd" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                                <asp:CheckBox ID="chkOnlinePaymentEdit" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                                <asp:CheckBox ID="chkOnlinePaymentDelete" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp; 
                                                                                                <asp:CheckBox ID="chkOnlinePaymentView" OnCheckedChanged="chkOnlinePaymentView_CheckedChanged" AutoPostBack="true" CssClass="css-checkbox" runat="server" Text="View" />
                                                                                                <asp:CheckBox ID="chkOnlinePaymentReport" CssClass="css-checkbox" runat="server" Text="Report" />&nbsp;                                      
                                                                                                <asp:CheckBox ID="chkOnlinePaymentApprove" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Approve" />&nbsp;
                                                                                            </td>
                                                                                        </tr>--%>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">Make Deposit
                                                                                            </td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkMakeDepositAdd" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                                <asp:CheckBox ID="chkMakeDepositEdit" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                                <asp:CheckBox ID="chkMakeDepositDelete" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                                <asp:CheckBox ID="chkMakeDepositView" OnCheckedChanged="chkMakeDepositView_CheckedChanged" AutoPostBack="true" CssClass="css-checkbox" runat="server" Text="View" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">Collections
                                                                                            </td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkCollectionsAdd" Style="display: none;" CssClass="css-checkbox" runat="server" Text="Add" />
                                                                                                <asp:CheckBox ID="chkCollectionsEdit" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                                <asp:CheckBox ID="chkCollectionsReport" CssClass="css-checkbox" runat="server" Text="Report" />&nbsp;                                      
                                                                                                <asp:CheckBox ID="chkCollectionsDelete" Style="display: none;" CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;                                      
                                                                                                <asp:CheckBox ID="chkCollectionsView" OnCheckedChanged="chkCollectionsView_CheckedChanged" AutoPostBack="true" CssClass="css-checkbox" runat="server" Text="View" />

                                                                                            </td>

                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkWriteOff" CssClass="css-checkbox" AutoPostBack="true" OnCheckedChanged="chkWriteOff_CheckedChanged" runat="server" Text="Write off" />
                                                                                            </td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkCreditHold" CssClass="css-checkbox" AutoPostBack="true" OnCheckedChanged="chkCreditHold_CheckedChanged" runat="server" Text="Credit Hold" />&nbsp;
                                                                                                <asp:CheckBox ID="chkCreditFlag" CssClass="css-checkbox" AutoPostBack="true" OnCheckedChanged="chkCreditFlag_CheckedChanged" runat="server" Text="Credit Flag" />&nbsp;
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
                                                                                <asp:CheckBox ID="chkRecurring" CssClass="css-checkbox" AutoPostBack="true" OnCheckedChanged="chkRecurring_CheckedChanged" runat="server" Font-Bold="true" Text="Recurring  module" />
                                                                            </div>
                                                                            <div class="input-field col s12">
                                                                                <div class="row">
                                                                                    <table>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">Recurring Contracts</td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkRecContractsAdd" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                                <asp:CheckBox ID="chkRecContractsEdit" OnClick='checkCustomerModule()' CssClass="css-checkbox " runat="server" Text="Edit" />&nbsp;
                                                                                                <asp:CheckBox ID="chkRecContractsDelete" OnClick='checkCustomerModule()' CssClass="css-checkbox " runat="server" Text="Delete" />&nbsp;
                                                                                                <asp:CheckBox ID="chkRecContractsView" OnCheckedChanged="chkRecContractsView_CheckedChanged" AutoPostBack="true" CssClass="css-checkbox" runat="server" Text="View" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">Safety Tests</td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkSafetyTestsAdd" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                                <asp:CheckBox ID="chkSafetyTestsEdit" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                                <asp:CheckBox ID="chkSafetyTestsDelete" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                                <asp:CheckBox ID="chkSafetyTestsView" AutoPostBack="true" OnCheckedChanged="chkSafetyTestsView_CheckedChanged" CssClass="css-checkbox" runat="server" Text="View" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">Recurring Invoices</td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkRecInvoicesAdd" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                                <asp:CheckBox ID="chkRecInvoicesEdit" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Style="display: none;" Text="Edit" />&nbsp;
                                                                                                <asp:CheckBox ID="chkRecInvoicesDelete" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                                <asp:CheckBox ID="chkRecInvoicesView" AutoPostBack="true" OnCheckedChanged="chkRecInvoicesView_CheckedChanged" CssClass="css-checkbox" runat="server" Text="View" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">Recurring Tickets</td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkRecTicketsAdd" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                                <asp:CheckBox ID="chkRecTicketsEdit" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Style="display: none;" Text="Edit" />&nbsp;
                                                                                                <asp:CheckBox ID="chkRecTicketsDelete" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                                <asp:CheckBox ID="chkRecTicketsView" AutoPostBack="true" OnCheckedChanged="chkRecTicketsView_CheckedChanged" CssClass="css-checkbox" runat="server" Text="View" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">Renew/Escalate</td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkRenewEscalateAdd" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                                <asp:CheckBox ID="chkRenewEscalateEdit" CssClass="css-checkbox" Style="display: none;" runat="server" Text="Edit" />&nbsp;
                                                                                                <asp:CheckBox ID="chkRenewEscalateDelete" CssClass="css-checkbox" Style="display: none;" runat="server" Text="Delete" />&nbsp;
                                                                                                <asp:CheckBox ID="chkRenewEscalateView" AutoPostBack="true" OnCheckedChanged="chkRenewEscalateView_CheckedChanged" CssClass="css-checkbox" runat="server" Text="View" />
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
                                                                                <asp:CheckBox ID="chkSchedule" CssClass="css-checkbox" AutoPostBack="true" OnCheckedChanged="chkSchedule_CheckedChanged" runat="server" Font-Bold="true" Text="Schedule module" />
                                                                            </div>
                                                                            <div class="input-field col s12">
                                                                                <div class="row">
                                                                                    <table>
                                                                                        <%--<tr runat="server">
                                                                                            <td runat="server">Schedule</td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkScheduleadd"  CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                           <asp:CheckBox ID="chkScheduleedit"   CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                           <asp:CheckBox ID="chkScheduledelete"   CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
 						                                                                   <asp:CheckBox ID="chkSchedulereport"   CssClass="css-checkbox" runat="server" Text="Report" />&nbsp;
                                                                                           <asp:CheckBox ID="chkScheduleview"  OnCheckedChanged="chkScheduleview_CheckedChanged" AutoPostBack="true"  CssClass="css-checkbox"  runat="server" Text="View" />
                                                                                            </td>
                                                                                        </tr>--%>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">Ticket
                                                                                            </td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkTicketListAdd" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                                <asp:CheckBox ID="chkTicketListEdit" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                                <asp:CheckBox ID="chkTicketListDelete" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                                <asp:CheckBox ID="chkTicketListReport" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Report" />&nbsp;
                                                                                                <asp:CheckBox ID="chkTicketListView" OnCheckedChanged="chkTicketListView_CheckedChanged" AutoPostBack="true" CssClass="css-checkbox" runat="server" Text="View" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">Completed Ticket</td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkResolveTicketAdd" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                                <asp:CheckBox ID="chkResolveTicketEdit" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                                <asp:CheckBox ID="chkResolveTicketDelete" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                                <asp:CheckBox ID="chkResolveTicketReport" Style="display: none" CssClass="css-checkbox" runat="server" Text="Report" />&nbsp;
                                                                                                <asp:CheckBox ID="chkResolveTicketView" AutoPostBack="true" OnCheckedChanged="chkResolveTicketView_CheckedChanged" CssClass="css-checkbox" runat="server" Text="View" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkMassReview" AutoPostBack="true" OnCheckedChanged="chkMassReview_CheckedChanged" CssClass="css-checkbox" runat="server" Text="Mass Review Ticket" />
                                                                                                <asp:CheckBox ID="chkMassTimesheetCheck" AutoPostBack="true" OnCheckedChanged="chkMassTimesheetCheck_CheckedChanged" CssClass="css-checkbox" runat="server" Text="Mass Review Timesheet" />
                                                                                            </td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkMassPayrollTicket" AutoPostBack="true" CssClass="css-checkbox" runat="server" OnCheckedChanged="chkMassPayrollTicket_CheckedChanged" Text="Mass Review Payroll" /></td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkScheduleBoard" CssClass="css-checkbox" AutoPostBack="false" runat="server" Text="Schedule Board"  />
                                                                                            </td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkRouteBuilderView" OnCheckedChanged="chkRouteBuilderView_CheckedChanged" AutoPostBack="true" CssClass="css-checkbox" runat="server" Text="Route Builder" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkTimestampFix" AutoPostBack="true" OnCheckedChanged="chkTimestampFix_CheckedChanged" CssClass="css-checkbox" runat="server" Text="Timestamps Fixed" />
                                                                                            </td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkTimesheetadd" Style="display: none" CssClass="css-checkbox" runat="server" Text="Add" />
                                                                                                <asp:CheckBox ID="chkTimesheetedit" Style="display: none" CssClass="css-checkbox" runat="server" Text="Edit" />
                                                                                                <asp:CheckBox ID="chkTimesheetdelete" Style="display: none" CssClass="css-checkbox" runat="server" Text="Delete" />
                                                                                                <asp:CheckBox ID="chkTimesheetreport" Style="display: none" CssClass="css-checkbox" runat="server" Text="Report" />
                                                                                                <asp:CheckBox ID="chkTimesheetview" OnCheckedChanged="chkTimesheetview_CheckedChanged" AutoPostBack="true" CssClass="css-checkbox" runat="server" Text="Timesheet Entry" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkETimesheetadd" Style="display: none" CssClass="css-checkbox" runat="server" Text="Add" />
                                                                                                <asp:CheckBox ID="chkETimesheetedit" Style="display: none" CssClass="css-checkbox" runat="server" Text="Edit" />
                                                                                                <asp:CheckBox ID="chkETimesheetdelete" Style="display: none" CssClass="css-checkbox" runat="server" Text="Delete" />
                                                                                                <asp:CheckBox ID="chkETimesheetreport" Style="display: none" CssClass="css-checkbox" runat="server" Text="Report" />
                                                                                                <asp:CheckBox ID="chkETimesheetview" OnCheckedChanged="chkETimesheetview_CheckedChanged" AutoPostBack="true" CssClass="css-checkbox" runat="server" Text="e-Timesheet (Payroll data)" />
                                                                                            </td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkMapView" OnCheckedChanged="chkMapView_CheckedChanged" AutoPostBack="true" CssClass="css-checkbox" runat="server" Text="Map" />
                                                                                                <asp:CheckBox ID="chkTicketVoidPermission" AutoPostBack="true" CssClass="css-checkbox" runat="server" Text="Void" />
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
                                                                                    <asp:CheckBox ID="chkProjectmodule" CssClass="css-checkbox" AutoPostBack="true" runat="server" OnCheckedChanged="chkProjectmodule_CheckedChanged" Text="Project module" />
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
                                                                                                <asp:CheckBox ID="chkViewFinance" CssClass="css-checkbox" runat="server" Text="Project Finance" /></td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkProjectManager" runat="server" CssClass="css-checkbox" Text="Project Manager" />
                                                                                            </td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkAssignedProject" runat="server" CssClass="css-checkbox" Text="Assigned Projects" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkLaborExpense" runat="server" CssClass="css-checkbox" Text="Recalculate Labor Expense" />
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
                                                                                    <asp:CheckBox ID="chkInventorymodule" CssClass="css-checkbox" runat="server" AutoPostBack="true" OnCheckedChanged="chkInventorymodule_CheckedChanged" Text="Inventory module" />
                                                                                </b>
                                                                            </div>
                                                                            <div class="input-field col s12">
                                                                                <div class="row">
                                                                                    <table>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">Inventory Item List</td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkInventoryItemadd" CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                                <asp:CheckBox ID="chkInventoryItemedit" CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                                <asp:CheckBox ID="chkInventoryItemdelete" CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                                <asp:CheckBox ID="chkInventoryItemview" CssClass="css-checkbox" runat="server" Text="View" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">Inventory Adjustment</td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkInventoryAdjustmentadd" CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                                <asp:CheckBox ID="chkInventoryAdjustmentedit" CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                                <asp:CheckBox ID="chkInventoryAdjustmentdelete" CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                                <asp:CheckBox ID="chkInventoryAdjustmentview" CssClass="css-checkbox" runat="server" Text="View" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">Inventory WareHouse</td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkInventoryWareHouseadd" CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                                <asp:CheckBox ID="chkInventoryWareHouseedit" CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                                <asp:CheckBox ID="chkInventoryWareHousedelete" CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                                <asp:CheckBox ID="chkInventoryWareHouseview" CssClass="css-checkbox" runat="server" Text="View" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">Inventory setup</td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkInventorysetupadd" CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                                <asp:CheckBox ID="chkInventorysetupedit" CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                                <asp:CheckBox ID="chkInventorysetupdelete" CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                                <asp:CheckBox ID="chkInventorysetupview" CssClass="css-checkbox" runat="server" Text="View" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">Inventory Finance</td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkInventoryFinanceAdd" CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                                <asp:CheckBox ID="chkInventoryFinanceedit" CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                                <asp:CheckBox ID="chkInventoryFinancedelete" CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                                <asp:CheckBox ID="chkInventoryFinanceview" CssClass="css-checkbox" runat="server" Text="View" />
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
                                                                                <asp:CheckBox ID="chkSalesMgr" CssClass="css-checkbox" AutoPostBack="true" OnCheckedChanged="chkSalesMgr_CheckedChanged" runat="server" Font-Bold="true" Text="Sales module" />
                                                                            </div>
                                                                            <div class="input-field col s12">
                                                                                <div class="row">
                                                                                    <table>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">Leads </td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkLeadAdd" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                                <asp:CheckBox ID="chkLeadEdit" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                                <asp:CheckBox ID="chkLeadDelete" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                                <asp:CheckBox ID="chkLeadReport" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Report" />&nbsp;
                                                                                                <asp:CheckBox ID="chkLeadView" OnCheckedChanged="chkLeadView_CheckedChanged" AutoPostBack="true" CssClass="css-checkbox" runat="server" Text="View" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">Opportunities </td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkOppAdd" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                                <asp:CheckBox ID="chkOppEdit" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                                <asp:CheckBox ID="chkOppDelete" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                                <asp:CheckBox ID="chkOppReport" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Report" />&nbsp;
                                                                                                <asp:CheckBox ID="chkOppView" OnCheckedChanged="chkOppView_CheckedChanged" AutoPostBack="true" CssClass="css-checkbox" runat="server" Text="View" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">Estimate  </td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkEstimateAdd" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                                <asp:CheckBox ID="chkEstimateEdit" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                                <asp:CheckBox ID="chkEstimateDelete" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                                <asp:CheckBox ID="chkEstimateReport" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Report" />&nbsp;
                                                                                                <asp:CheckBox ID="chkEstimateView" OnCheckedChanged="chkEstimateView_CheckedChanged" AutoPostBack="true" CssClass="css-checkbox" runat="server" Text="View" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkCompleteTask" OnCheckedChanged="chkCompleteTask_CheckedChanged" AutoPostBack="true" CssClass="css-checkbox" runat="server" Text="Complete Task" />
                                                                                            </td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkFollowUp" OnCheckedChanged="chkFollowUp_CheckedChanged" AutoPostBack="true" CssClass="css-checkbox" runat="server" Text="Task FollowUp" />
                                                                                                <asp:CheckBox ID="chkTasks" OnCheckedChanged="chkTasks_CheckedChanged" AutoPostBack="true" CssClass="css-checkbox" runat="server" Text="Tasks" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkConvertEstimate" OnCheckedChanged="chkConvertEstimate_CheckedChanged" AutoPostBack="true" CssClass="css-checkbox" runat="server" Text="Convert Estimate" />
                                                                                            </td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkSalesSetup" OnCheckedChanged="chkSalesSetup_CheckedChanged" AutoPostBack="true" CssClass="css-checkbox" runat="server" Text="Sales Setup" />

                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">
                                                                                                <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                                                                    <ContentTemplate>--%>
                                                                                                <asp:CheckBox ID="chkSalesperson" runat="server" AutoPostBack="True" Text="Sales person"
                                                                                                    OnCheckedChanged="chkSalesperson_CheckedChanged" CssClass="css-checkbox" />
                                                                                                <%--</ContentTemplate>
                                                                                                </asp:UpdatePanel>--%>
                                                                                            </td>
                                                                                            <td runat="server">
                                                                                                <%--<asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                                                                                    <ContentTemplate>--%>
                                                                                                <asp:CheckBox ID="chkSalesAssigned" runat="server" AutoPostBack="false" Enabled="False" Text="Sales Assigned" CssClass="css-checkbox" />
                                                                                                <%--</ContentTemplate>
                                                                                                </asp:UpdatePanel>--%>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td runat="server" colspan="2">
                                                                                                <%--<asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                                                                                    <ContentTemplate>--%>
                                                                                                <asp:CheckBox ID="chkNotification" Enabled="False" runat="server" Text="Opportunity Notification"
                                                                                                    AutoPostBack="false" CssClass="css-checkbox" />
                                                                                                <%--</ContentTemplate>
                                                                                                </asp:UpdatePanel>--%>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td runat="server" colspan="2">
                                                                                                <%--<asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                                                                                    <ContentTemplate>--%>
                                                                                                <asp:CheckBox ID="chkEstApprovalStatus" runat="server" Text="Estimate Approve Proposal"
                                                                                                    AutoPostBack="false" CssClass="css-checkbox" />
                                                                                                <%--</ContentTemplate>
                                                                                                </asp:UpdatePanel>--%>
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
                                                                                <asp:CheckBox ID="chkAccountPayable" CssClass="css-checkbox" AutoPostBack="true" OnCheckedChanged="chkAccountPayable_CheckedChanged" runat="server" Font-Bold="true" Text="AP module" />
                                                                            </div>
                                                                            <div class="input-field col s12">
                                                                                <div class="row">
                                                                                    <table>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">Vendors</td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkVendorsAdd" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                                <asp:CheckBox ID="chkVendorsEdit" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                                <asp:CheckBox ID="chkVendorsDelete" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                                <asp:CheckBox ID="chkVendorsView" AutoPostBack="true" OnCheckedChanged="chkVendorsView_CheckedChanged" CssClass="css-checkbox" runat="server" Text="View" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">Bills</td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkAddBills" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                            <asp:CheckBox ID="chkEditBills" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                            <asp:CheckBox ID="chkDeleteBills" CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                            <asp:CheckBox ID="chkViewBills" AutoPostBack="true" OnCheckedChanged="chkViewBills_CheckedChanged" CssClass="css-checkbox" runat="server" Text="View" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">Manage Checks</td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkAddManageChecks" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                                <asp:CheckBox ID="chkEditManageChecks" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                                <asp:CheckBox ID="chkDeleteManageChecks" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                                <asp:CheckBox ID="chkViewManageChecks" AutoPostBack="true" OnCheckedChanged="chkViewManageChecks_CheckedChanged" CssClass="css-checkbox" runat="server" Text="View" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td runat="server" colspan="2">
                                                                                                <asp:CheckBox ID="chkShowBankBalances" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Show Bank Balances" />
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
                                                                                <asp:CheckBox ID="chkFinancialmodule" CssClass="css-checkbox" AutoPostBack="true" OnCheckedChanged="chkFinancialmodule_CheckedChanged" runat="server" Font-Bold="true" Text="Financial module" />
                                                                            </div>
                                                                            <div class="input-field col s12">
                                                                                <div class="row">
                                                                                    <table>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">Chart of Accounts</td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkChartAdd" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                                <asp:CheckBox ID="chkChartEdit" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                                <asp:CheckBox ID="chkChartDelete" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                                <asp:CheckBox ID="chkChartView" AutoPostBack="true" OnCheckedChanged="chkChartView_CheckedChanged" CssClass="css-checkbox" runat="server" Text="View" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">Journal Entry</td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkJournalEntryAdd" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                            <asp:CheckBox ID="chkJournalEntryEdit" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                            <asp:CheckBox ID="chkJournalEntryDelete" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                            <asp:CheckBox ID="chkJournalEntryView" AutoPostBack="true" OnCheckedChanged="chkJournalEntryView_CheckedChanged" CssClass="css-checkbox" runat="server" Text="View" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">Bank Reconciliation</td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkBankAdd" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                                <asp:CheckBox ID="chkBankEdit" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                                <asp:CheckBox ID="chkBankDelete" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                                <asp:CheckBox ID="chkBankView" AutoPostBack="true" OnCheckedChanged="chkBankView_CheckedChanged" CssClass="css-checkbox" runat="server" Text="View" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td runat="server" colspan="2">
                                                                                                <asp:CheckBox ID="chkFinanceStatement" CssClass="css-checkbox" runat="server" Text="Financial Statement Module" />
                                                                                            </td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkFinanceMgr" CssClass="css-checkbox" runat="server" Style="display: none" Text="Financial Manager" /></td>
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
                                                                                <asp:CheckBox ID="chkBillingmodule" CssClass="css-checkbox" AutoPostBack="true" OnCheckedChanged="chkBillingmodule_CheckedChanged" runat="server" Font-Bold="true" Text="Billing module" />
                                                                            </div>
                                                                            <div class="input-field col s12">
                                                                                <div class="row">
                                                                                    <table>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">Invoices</td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkInvoicesAdd" OnClick='checkCustomerModule()' CssClass="css-checkbox Billingmodule" runat="server" Text="Add" />&nbsp;
                                                                                                <asp:CheckBox ID="chkInvoicesEdit" OnClick='checkCustomerModule()' CssClass="css-checkbox Billingmodule" runat="server" Text="Edit" />&nbsp;
                                                                                                <asp:CheckBox ID="chkInvoicesDelete" OnClick='checkCustomerModule()' CssClass="css-checkbox Billingmodule" runat="server" Text="Delete" />&nbsp;
                                                                                                <asp:CheckBox ID="chkInvoicesView" OnCheckedChanged="chkInvoicesView_CheckedChanged" AutoPostBack="true" CssClass="css-checkbox Billingmodule" runat="server" Text="View" />

                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">Billing Codes</td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkBillingcodesAdd" OnClick='checkCustomerModule()' CssClass="css-checkbox Billingmodule" runat="server" Text="Add" />&nbsp;
                                                                                                <asp:CheckBox ID="chkBillingcodesEdit" OnClick='checkCustomerModule()' CssClass="css-checkbox Billingmodule" runat="server" Text="Edit" />&nbsp;
                                                                                                <asp:CheckBox ID="chkBillingcodesDelete" OnClick='checkCustomerModule()' CssClass="css-checkbox Billingmodule" runat="server" Text="Delete" />&nbsp;
                                                                                                <asp:CheckBox ID="chkBillingcodesView" AutoPostBack="true" OnCheckedChanged="chkBillingcodesView_CheckedChanged" CssClass="css-checkbox Billingmodule" runat="server" Text="View" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server" id="trOnlinePayment">
                                                                                            <td runat="server">Online Payment</td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkPaymentHistoryView" AutoPostBack="true" OnCheckedChanged="chkPaymentHistoryView_CheckedChanged" CssClass="css-checkbox Billingmodule" runat="server" Text="View" />
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
                                                                                <asp:CheckBox ID="chkPurchasingmodule" AutoPostBack="true" OnCheckedChanged="chkPurchasingmodule_CheckedChanged" CssClass="css-checkbox" runat="server" Font-Bold="true" Text="Purchasing module" />
                                                                            </div>
                                                                            <div class="input-field col s12">
                                                                                <div class="row">
                                                                                    <table>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">PO</td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkPOAdd" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                                <asp:CheckBox ID="chkPOEdit" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                                <asp:CheckBox ID="chkPODelete" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                                <asp:CheckBox ID="chkPOView" AutoPostBack="true" OnCheckedChanged="chkPOView_CheckedChanged" CssClass="css-checkbox" runat="server" Text="View" />
                                                                                            </td>
                                                                                        </tr>

                                                                                        <tr runat="server">
                                                                                            <td runat="server">Receive PO</td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkRPOAdd" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                                <asp:CheckBox ID="chkRPOEdit" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                                <asp:CheckBox ID="chkRPODelete" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                                <asp:CheckBox ID="chkRPOView" AutoPostBack="true" OnCheckedChanged="chkRPOView_CheckedChanged" CssClass="css-checkbox" runat="server" Text="View" />
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
                                                                                <asp:CheckBox ID="chkProgram" CssClass="css-checkbox" OnCheckedChanged="chkProgram_CheckedChanged" AutoPostBack="true" runat="server" Font-Bold="true" Text="Program Module" />
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
                                                                                                <asp:CheckBox ID="chkDocumentAdd" CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                                <asp:CheckBox ID="chkDocumentEdit" CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                                <asp:CheckBox ID="chkDocumentDelete" CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                                <asp:CheckBox ID="chkDocumentView" CssClass="css-checkbox" runat="server" Text="View" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">Contact</td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkContactAdd" CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                                <asp:CheckBox ID="chkContactEdit" CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                                <asp:CheckBox ID="chkContactDelete" CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                                <asp:CheckBox ID="chkContactView" CssClass="css-checkbox" runat="server" Text="View" />
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
                                                                                <asp:CheckBox ID="payrollModulchck" CssClass="css-checkbox" AutoPostBack="true" OnCheckedChanged="chkPayrollMgr_CheckedChanged" runat="server" Font-Bold="true" Text="Payroll module" />
                                                                            </div>
                                                                            <div class="input-field col s12">
                                                                                <div class="row">
                                                                                    <table>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">Employees </td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="empAdd" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                                <asp:CheckBox ID="empEdit" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                                <asp:CheckBox ID="empDelete" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                                <asp:CheckBox ID="empView" OnCheckedChanged="chkempView_CheckedChanged" AutoPostBack="true" CssClass="css-checkbox" runat="server" Text="View" />&nbsp;
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">Run Payroll </td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="runAdd" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                                <asp:CheckBox ID="runEdit" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                                <asp:CheckBox ID="runDelete" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                                <asp:CheckBox ID="runView" OnCheckedChanged="chkrunpayrollView_CheckedChanged" AutoPostBack="true" CssClass="css-checkbox" runat="server" Text="View" />&nbsp;
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">Payroll Checks  </td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="payrollchckAdd" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                                <asp:CheckBox ID="payrollchckEdit" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                                <asp:CheckBox ID="payrollchckDelete" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                                <asp:CheckBox ID="payrollchckView" OnCheckedChanged="chkpayrollcheckView_CheckedChanged" AutoPostBack="true" CssClass="css-checkbox" runat="server" Text="View" />&nbsp;
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">Payroll Form  </td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="payrollformAdd" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                                <asp:CheckBox ID="payrollformEdit" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                                <asp:CheckBox ID="payrollformDelete" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                                <asp:CheckBox ID="payrollformView" OnCheckedChanged="chkpayrollformView_CheckedChanged" AutoPostBack="true" CssClass="css-checkbox" runat="server" Text="View" />&nbsp;
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">Wages  </td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="wagesadd" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                                <asp:CheckBox ID="wagesEdit" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                                <asp:CheckBox ID="wagesDelete" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                                <asp:CheckBox ID="wagesView" OnCheckedChanged="chkwagesView_CheckedChanged" AutoPostBack="true" CssClass="css-checkbox" runat="server" Text="View" />&nbsp;
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">Deductions  </td>
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="deductionsAdd" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Add" />&nbsp;
                                                                                                <asp:CheckBox ID="deductionsEdit" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Edit" />&nbsp;
                                                                                                <asp:CheckBox ID="deductionsDelete" OnClick='checkCustomerModule()' CssClass="css-checkbox" runat="server" Text="Delete" />&nbsp;
                                                                                                <asp:CheckBox ID="deductionsView" OnCheckedChanged="chkdeductionView_CheckedChanged" AutoPostBack="true" CssClass="css-checkbox" runat="server" Text="View" />&nbsp;
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr runat="server">
                                                                                            <td runat="server">
                                                                                                <asp:CheckBox ID="chkMassPayrollTicket1" AutoPostBack="true" CssClass="css-checkbox" runat="server" OnCheckedChanged="chkMassPayrollTicket1_CheckedChanged" Text="Mass Review Payroll" />
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

                                        <li>
                                            <div id="accrdPay" class="collapsible-header accrd accordian-text-custom"><i class="mdi-editor-attach-money"></i>Payment</div>
                                            <div class="collapsible-body">
                                                <div class="form-content-wrap form-content-wrapwd">
                                                    <div class="form-content-pd">
                                                        <div class="form-section-row">
                                                            <div class="section-ttle">Details</div>
                                                            <div class="form-section3">
                                                                <div class="input-field col s5">
                                                                    <div class="row">
                                                                        <label for="empID">Emp ID</label>
                                                                        <asp:TextBox ID="txtEmpID" runat="server" MaxLength="50"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="input-field col s2">
                                                                    <div class="row">
                                                                        &nbsp;
                                                                    </div>
                                                                </div>
                                                                <div class="input-field col s5">
                                                                    <div class="row">
                                                                        <label for="txtSSNSIN">SSN/SIN</label>
                                                                        <asp:TextBox ID="txtSSNSIN" runat="server" MaxLength="11" placeholder="xxx-xx-xxxx"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="input-field col s12">
                                                                    <div class="row">
                                                                        <label class="drpdwn-label">Gender</label>

                                                                        <asp:DropDownList ID="ddlGender" runat="server" CssClass="browser-default">
                                                                            <asp:ListItem Value="N/A">N/A</asp:ListItem>
                                                                            <asp:ListItem Value="Male">Male</asp:ListItem>
                                                                            <asp:ListItem Value="Female">Female</asp:ListItem>
                                                                        </asp:DropDownList>


                                                                    </div>
                                                                </div>


                                                                <div class="input-field col s12">
                                                                    <div class="row">
                                                                        <label class="drpdwn-label">Ethnicity</label>
                                                                        <asp:DropDownList ID="ddlEthnicity" runat="server" CssClass="browser-default">
                                                                            <asp:ListItem Value="N/A">N/A</asp:ListItem>
                                                                            <asp:ListItem Value="White">White</asp:ListItem>
                                                                            <asp:ListItem Value="Black (not of Hispanic Origin)">Black (not of Hispanic origin)</asp:ListItem>
                                                                            <asp:ListItem Value="Hispanic">Hispanic</asp:ListItem>
                                                                            <asp:ListItem Value="Asian or Pacific Islander">Asian or Pacific Islander</asp:ListItem>
                                                                            <asp:ListItem Value="American Indian or Alaskan Native">American Indian or Alaskan Native</asp:ListItem>
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
                                                                        <label for="txtDateOfBirth">Date Of Birth</label>
                                                                        <asp:TextBox ID="txtDateOfBirth" runat="server" CssClass="txtDateOfBirth datepicker_mom" MaxLength="10"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="input-field col s12">
                                                                        <div class="row">
                                                                            <label for="sDate">Date of Hiring<span class="reqd">*</span></label>
                                                                            <%--<input id="sDate" type="text" class="datepicker_mom">--%>
                                                                            <asp:TextBox ID="txtHireDt" runat="server" CssClass="txtHireDt datepicker_mom validate" MaxLength="10"></asp:TextBox>
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server"
                                                                                ControlToValidate="txtHireDt" Display="None" ErrorMessage="Date of Hiring Required"
                                                                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                            <asp:ValidatorCalloutExtender
                                                                                ID="RequiredFieldValidator10_ValidatorCalloutExtender" PopupPosition="TopLeft" runat="server" Enabled="True" CssClass="valiateField"
                                                                                TargetControlID="RequiredFieldValidator10">
                                                                            </asp:ValidatorCalloutExtender>
                                                                        </div>
                                                                    </div>

                                                                    <div class="input-field col s12">
                                                                        <div class="row">
                                                                            <label for="txtTerminationDt">Date of Termination</label>
                                                                            <asp:TextBox ID="txtTerminationDt" runat="server" CssClass="datepicker_mom" MaxLength="10"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                            </div>
                                                            <div class="form-section3-blank">
                                                                &nbsp;
                                                            </div>
                                                             <div class="form-section3">
                                                                <div class="input-field col s5">
                                                                    <div class="row">
                                                                         <label class="drpdwn-label">Payment Method</label>
                                                                            <asp:UpdatePanel ID="UpdatePanel20" runat="server">
                                                                                <ContentTemplate>
                                                                                    <asp:DropDownList ID="ddlPayMethod" runat="server" CssClass="browser-default"
                                                                                        AutoPostBack="True" OnSelectedIndexChanged="ddlPayMethod_SelectedIndexChanged1">
                                                                                        <asp:ListItem Value="-1">Select</asp:ListItem>
                                                                                        <asp:ListItem Value="0">Salaried</asp:ListItem>
                                                                                        <asp:ListItem Value="1">Hourly</asp:ListItem>
                                                                                        <asp:ListItem Value="2">Fixed Hours</asp:ListItem>
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
                                                                <div class="input-field col s5">
                                                                    <div class="row">
                                                                        <label class="drpdwn-label">Payment Period</label>
                                                                            <asp:DropDownList ID="ddlPayPeriod" runat="server" CssClass="browser-default">
                                                                                <asp:ListItem Value="-1">Select</asp:ListItem>
                                                                                <asp:ListItem Value="0">Weekly</asp:ListItem>
                                                                                <asp:ListItem Value="1">Bi-Weekly</asp:ListItem>
                                                                                <asp:ListItem Value="2">Semi-Monthly</asp:ListItem>
                                                                                <asp:ListItem Value="3">Monthly</asp:ListItem>
                                                                                <asp:ListItem Value="4">Semi-Annually</asp:ListItem>
                                                                                <asp:ListItem Value="5">Annually</asp:ListItem>
                                                                            </asp:DropDownList>   
                                                                    </div>
                                                                </div>
                                                                 <div class="input-field col s5">
                                                                    <div class="row">
                                                                          <label for="txtAmount" id="lblAmount" runat="server" class="lblAmount">Amount</label>
                                                                            <asp:TextBox ID="txtAmount" CssClass="txtAmount" runat="server" onkeyup="activeLabel('Amount');"
                                                                                MaxLength="28" step="any"></asp:TextBox>
                                                                            <asp:FilteredTextBoxExtender ID="txtAmount_FilteredTextBoxExtender" runat="server"
                                                                                Enabled="True" TargetControlID="txtAmount" ValidChars="1234567890.-">
                                                                            </asp:FilteredTextBoxExtender>
                                                                    </div>
                                                                </div>
                                                                <div class="input-field col s2">
                                                                    <div class="row">
                                                                        &nbsp;
                                                                    </div>
                                                                </div>
                                                                <div class="input-field col s5">
                                                                    <div class="row">
                                                                       <label for="txtHourlyRate" id="lblHourlyR" runat="server">Hourly Rate</label>
                                                                            <asp:TextBox ID="txtHourlyRate" runat="server"
                                                                                MaxLength="28" step="any"></asp:TextBox>
                                                                            <asp:FilteredTextBoxExtender ID="txtHourlyRate_FilteredTextBoxExtender" runat="server"
                                                                                Enabled="True" TargetControlID="txtHourlyRate" ValidChars="1234567890.-">
                                                                            </asp:FilteredTextBoxExtender>
                                                                    </div>
                                                                </div>
                                                                 <div class="input-field col s5">
                                                                    <div class="row">
                                                                         <label for="txtHours" id="lblHours" runat="server" class="lblHours">Hours</label>
                                                                            <asp:TextBox ID="txtHours" CssClass="txtHours" runat="server" onkeyup="activeLabel('Hours');"
                                                                                MaxLength="28" step="any" TabIndex="20" Enabled="false"></asp:TextBox>
                                                                            <asp:FilteredTextBoxExtender ID="txtHours_FilteredTextBoxExtender" runat="server"
                                                                                Enabled="True" TargetControlID="txtHours" ValidChars="1234567890.-">
                                                                            </asp:FilteredTextBoxExtender>
                                                                    </div>
                                                                </div>
                                                                <div class="input-field col s2">
                                                                    <div class="row">
                                                                        &nbsp;
                                                                    </div>
                                                                </div>
                                                                <div class="input-field col s5">
                                                                    <div class="row">
                                                                         <label for="txtMileageRate">Mileage Rate</label>
                                                                            <asp:TextBox ID="txtMileageRate" runat="server"
                                                                                CssClass="form-control"
                                                                                MaxLength="28" step="any" TabIndex="27"></asp:TextBox>
                                                                            <asp:FilteredTextBoxExtender ID="txtMileageRate_FilteredTextBoxExtender" runat="server"
                                                                                Enabled="True" TargetControlID="txtMileageRate" ValidChars="1234567890.-">
                                                                            </asp:FilteredTextBoxExtender>
                                                                    </div>
                                                                </div>
                                                             </div>
                                                              <div class="form-section-row">
                                                                <div class="section-ttle" style="margin-bottom: 5px;">Wage</div>
                                                                <div class="btnlinks" style="margin-bottom: 10px;">
                                                                    <asp:LinkButton ID="btnCopyRate" runat="server" ToolTip="Copy selected rates to all" Text="Copy Rates"
                                                                        OnClientClick="return CheckCopyRate('gvWagePayRate.ClientID');" OnClick="btnCopyRate_Click">
                                                                    </asp:LinkButton>
                                                                </div>
                                                                <span class="tro trost">
                                                                    <asp:CheckBox ID="lnkChk" runat="server" OnCheckedChanged="lnkChk_CheckedChanged" AutoPostBack="True" Text="Incl. Inactive"></asp:CheckBox>
                                                                    <%--<asp:Label ID="lblChkSelect" runat="server">Incl. Closed</asp:Label>--%>
                                                                </span>
                                                                <div class="grid_container">
                                                                    <asp:HiddenField ID="hdnWageRate" runat="server" />
                                                                    <div class="RadGrid RadGrid_Material">

                                                                        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_gvWagePayRate" runat="server">
                                                                        </telerik:RadAjaxLoadingPanel>
                                                                        <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
                                                                            <script type="text/javascript">
                                                                                function pageLoad() {
                                                                                    var grid = $find("<%= gvWagePayRate.ClientID %>");
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
                                                                        <telerik:RadAjaxPanel ID="UpdatePanel6" runat="server" LoadingPanelID="RadAjaxLoadingPanel_gvWagePayRate" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                                            <%--    <ContentTemplate>--%>
                                                                            <telerik:RadGrid ID="gvWagePayRate" runat="server" AutoGenerateColumns="False" FilterType="CheckList"
                                                                                CssClass="gvWagePayRate table table-bordered table-striped table-condensed flip-content" margin-bottom="0px"
                                                                                AllowSorting="true"
                                                                                ShowFooter="true"
                                                                                OnItemCreated="gvWagePayRate_ItemCreated"
                                                                                OnRowCommand="gvWagePayRate_RowCommand"
                                                                                OnItemDataBound="gvWagePayRate_ItemDataBound"
                                                                                EnableViewState="true"
                                                                                OnNeedDataSource="gvWagePayRate_NeedDataSource">
                                                                                <AlternatingItemStyle CssClass="oddrowcolor" />
                                                                                <ActiveItemStyle CssClass="evenrowcolor" />
                                                                                <FooterStyle CssClass="footer" />
                                                                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                                    <Selecting AllowRowSelect="True"></Selecting>
                                                                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                                </ClientSettings>
                                                                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True" DataKeyNames="Reg">
                                                                                    <ColumnGroups>
                                                                                        <telerik:GridColumnGroup HeaderText="Pay Rate" HeaderStyle-CssClass="hdsPayRate" Name="PayRate" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                                                                                        <telerik:GridColumnGroup HeaderText="Burden Rate" HeaderStyle-CssClass="hdsBurdenRate" Name="BurdenRate" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                                                                                        <telerik:GridColumnGroup HeaderText="" Name="Checkbox" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                                                                                        <telerik:GridColumnGroup HeaderText="" Name="OrderNumber" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                                                                                        <telerik:GridColumnGroup HeaderText="" Name="Wage" HeaderStyle-CssClass="hdsWage" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                                                                                        <telerik:GridColumnGroup HeaderText="" Name="Status" HeaderStyle-CssClass="hdsWage" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                                                                                    </ColumnGroups>
                                                                                    <Columns>
                                                                                        <telerik:GridTemplateColumn AllowFiltering="false" ColumnGroupName="Checkbox" ItemStyle-Width="50px" HeaderStyle-Width="50px">
                                                                                            <HeaderTemplate>
                                                                                                <asp:ImageButton ID="imgBtnWageDelete" runat="server" CausesValidation="false" OnClick="imgBtnWageDelete_Click"
                                                                                                    OnClientClick="return CheckDelete('gvWagePayRate.ClientID');"
                                                                                                    ImageUrl="images/menu_delete.png" Width="13px" />
                                                                                            </HeaderTemplate>
                                                                                            <ItemTemplate>
                                                                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <asp:LinkButton ID="imgBtnWageAdd" runat="server" CommandName="AddWage" CausesValidation="False"
                                                                                                    CommandArgument="<%# ((GridItem) Container).ItemIndex %>"
                                                                                                    Width="30" OnClientClick="itemJSON();" OnClick="imgBtnWageAdd_Click" Text="Add"><i class="fa fa-plus-circle iconAdd_gvWagePayRate" ></i></asp:LinkButton>
                                                                                            </FooterTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn HeaderText="No." ColumnGroupName="OrderNumber" SortExpression="id" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center" AllowFiltering="false">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblNum" runat="server" Text='<%# ((GridItem) Container).ItemIndex + 1 %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>

                                                                                        <telerik:GridTemplateColumn HeaderText="Wage" ColumnGroupName="Wage" SortExpression="Wage" HeaderStyle-Width="180px" HeaderStyle-HorizontalAlign="Center" AllowFiltering="false">
                                                                                            <ItemTemplate>
                                                                                            

                                                                                                    <asp:TextBox ID="txtWage" Text='<%#Eval("fDesc") %>' runat="server" CssClass="wage-search" MaxLength="255"></asp:TextBox>
                                                                                                    <asp:HiddenField ID="hdnWageID"  Value='<%#Eval("Wage") %>'  runat="server" />
                                                                                                    <asp:HiddenField ID="hdnWageIDtext"  Value='<%#Eval("fDesc") %>'  runat="server" />
                                                                                                    <asp:HiddenField ID="hdnIsUsed" Value="0" runat="server" />
                                                                                

                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>

                                                                                        <telerik:GridTemplateColumn HeaderText="Status" ColumnGroupName="Status" SortExpression="Status" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" AllowFiltering="false">
                                                                                            <ItemTemplate>
                                                                                                
                                                                                                <asp:DropDownList ID="ddlWageStatus" runat="server" DataValueField="Status" DataTextField="Status" CssClass="browser-default"
                                                                                                    AutoPostBack="false"   Width="100%"  
                                                                                                    SelectedValue='<%# Eval("Status") %>'>
                                                                                                      <asp:ListItem Text="Active" Selected="True" Value="0"></asp:ListItem>
                                                                                                      <asp:ListItem Text="Inactive" Value="1"></asp:ListItem>
                                                                                                </asp:DropDownList>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>

                                                                                        <telerik:GridTemplateColumn HeaderText="Regular" SortExpression="Reg" ColumnGroupName="PayRate" HeaderStyle-HorizontalAlign="Center"
                                                                                            AutoPostBackOnFilter="true" ShowFilterIcon="false" FilterDelay="5" CurrentFilterFunction="EqualTo" DataField="Reg" UniqueName="Reg"
                                                                                            HeaderStyle-CssClass="PayRate" ItemStyle-CssClass="PayRate">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtReg" runat="server" Text='<%#Eval("Reg","{0:0.00}") %>' Width="100%" onkeypress="return isDecimalKey(this,event)"
                                                                                                    CssClass="form-control rate-content" onchange="showDecimalVal(this)" Style="text-align: right">
                                                                                                </asp:TextBox>
                                                                                                <%--<asp:Label ID="txtReg" runat="server" Text='<%#Eval("Reg","{0:0.00}") %>' Width="100%" ></asp:Label>--%>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>

                                                                                        <telerik:GridTemplateColumn HeaderText="Overtime" SortExpression="OT" ColumnGroupName="PayRate" HeaderStyle-HorizontalAlign="Center"
                                                                                            AutoPostBackOnFilter="true" ShowFilterIcon="false" FilterDelay="5" CurrentFilterFunction="EqualTo" DataField="OT" UniqueName="OT"
                                                                                            HeaderStyle-CssClass="PayRate" ItemStyle-CssClass="PayRate">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtOt" runat="server" Text='<%#Eval("OT","{0:0.00}")%>' Width="100%" onkeypress="return isDecimalKey(this,event)"
                                                                                                    CssClass="form-control rate-content" onchange="showDecimalVal(this)" Style="text-align: right">
                                                                                                </asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn HeaderText="1.7 Time" SortExpression="NT" ColumnGroupName="PayRate" HeaderStyle-HorizontalAlign="Center"
                                                                                            AutoPostBackOnFilter="true" ShowFilterIcon="false" FilterDelay="5" CurrentFilterFunction="EqualTo" DataField="NT" UniqueName="NT"
                                                                                            HeaderStyle-CssClass="PayRate" ItemStyle-CssClass="PayRate">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtNt" runat="server" Text='<%#Eval("NT","{0:0.00}")%>' Width="100%" onkeypress="return isDecimalKey(this,event)"
                                                                                                    CssClass="form-control rate-content"
                                                                                                    onchange="showDecimalVal(this)" Style="text-align: right">
                                                                                                </asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn HeaderText="Double Time" SortExpression="DT" ColumnGroupName="PayRate" HeaderStyle-HorizontalAlign="Center"
                                                                                            AutoPostBackOnFilter="true" ShowFilterIcon="false" FilterDelay="5" CurrentFilterFunction="EqualTo" DataField="DT" UniqueName="DT"
                                                                                            HeaderStyle-CssClass="PayRate" ItemStyle-CssClass="PayRate">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtDt" runat="server" Text='<%#Eval("DT","{0:0.00}")%>' Width="100%" onkeypress="return isDecimalKey(this,event)"
                                                                                                    CssClass="form-control rate-content" onchange="showDecimalVal(this)" Style="text-align: right">
                                                                                                </asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn HeaderText="Travel" SortExpression="TT" ColumnGroupName="PayRate" HeaderStyle-HorizontalAlign="Center"
                                                                                            AutoPostBackOnFilter="true" ShowFilterIcon="false" FilterDelay="5" CurrentFilterFunction="EqualTo" DataField="TT" UniqueName="TT"
                                                                                            HeaderStyle-CssClass="PayRate" ItemStyle-CssClass="PayRate">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtTt" runat="server" Text='<%#Eval("TT","{0:0.00}")%>' Width="100%" onkeypress="return isDecimalKey(this,event)"
                                                                                                    CssClass="form-control rate-content" onchange="showDecimalVal(this)" Style="text-align: right">
                                                                                                </asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn HeaderText="Regular" SortExpression="CReg" ColumnGroupName="BurdenRate" HeaderStyle-HorizontalAlign="Center"
                                                                                            AutoPostBackOnFilter="true" ShowFilterIcon="false" FilterDelay="5" CurrentFilterFunction="EqualTo" DataField="CReg" UniqueName="CReg"
                                                                                            HeaderStyle-CssClass="BurdenRate" ItemStyle-CssClass="BurdenRate">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtCReg" runat="server" Text='<%#Eval("CReg","{0:0.00}") %>' Width="100%" onkeypress="return isDecimalKey(this,event)"
                                                                                                    CssClass="form-control rate-content" onchange="showDecimalVal(this)" Style="text-align: right">
                                                                                                </asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn HeaderText="Overtime" SortExpression="COT" ColumnGroupName="BurdenRate" HeaderStyle-HorizontalAlign="Center"
                                                                                            AutoPostBackOnFilter="true" ShowFilterIcon="false" FilterDelay="5" CurrentFilterFunction="EqualTo" DataField="COT" UniqueName="COT"
                                                                                            HeaderStyle-CssClass="BurdenRate" ItemStyle-CssClass="BurdenRate">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtCOt" runat="server" Text='<%#Eval("COT","{0:0.00}")%>' Width="100%" onkeypress="return isDecimalKey(this,event)"
                                                                                                    CssClass="form-control rate-content" onchange="showDecimalVal(this)" Style="text-align: right">
                                                                                                </asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn HeaderText="1.7 Time" SortExpression="CNT" ColumnGroupName="BurdenRate" HeaderStyle-HorizontalAlign="Center"
                                                                                            AutoPostBackOnFilter="true" ShowFilterIcon="false" FilterDelay="500" CurrentFilterFunction="EqualTo" DataField="CNT" UniqueName="CNT"
                                                                                            HeaderStyle-CssClass="BurdenRate" ItemStyle-CssClass="BurdenRate">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtCNt" runat="server" Text='<%# Eval("CNT","{0:0.00}")%>' Width="100%" onkeypress="return isDecimalKey(this,event)"
                                                                                                    CssClass="form-control rate-content" onchange="showDecimalVal(this)" Style="text-align: right">
                                                                                                </asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn HeaderText="Double Time" SortExpression="CDT" ColumnGroupName="BurdenRate" HeaderStyle-HorizontalAlign="Center"
                                                                                            AutoPostBackOnFilter="true" ShowFilterIcon="false" FilterDelay="5" CurrentFilterFunction="EqualTo" DataField="CDT" UniqueName="CDT"
                                                                                            HeaderStyle-CssClass="BurdenRate" ItemStyle-CssClass="BurdenRate">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtCDt" runat="server" Text='<%#Eval("CDT","{0:0.00}")%>' Width="100%" onkeypress="return isDecimalKey(this,event)"
                                                                                                    CssClass="form-control rate-content" onchange="showDecimalVal(this)" Style="text-align: right">
                                                                                                </asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn HeaderText="Travel" SortExpression="CTT" ColumnGroupName="BurdenRate" HeaderStyle-HorizontalAlign="Center"
                                                                                            AutoPostBackOnFilter="true" ShowFilterIcon="false" FilterDelay="5" CurrentFilterFunction="EqualTo" DataField="CTT" UniqueName="CTT"
                                                                                            HeaderStyle-CssClass="BurdenRate" ItemStyle-CssClass="BurdenRate">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtCTt" runat="server" Text='<%#Eval("CTT","{0:0.00}")%>' Width="100%" onkeypress="return isDecimalKey(this,event)"
                                                                                                    CssClass="form-control rate-content" onchange="showDecimalVal(this)" Style="text-align: right">
                                                                                                </asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                    </Columns>
                                                                                </MasterTableView>
                                                                                <SelectedItemStyle CssClass="selectedrowcolor" />
                                                                                <FilterMenu CssClass="RadFilterMenu_CheckList">
                                                                                </FilterMenu>
                                                                            </telerik:RadGrid>
                                                                            <%--</ContentTemplate>--%>
                                                                        </telerik:RadAjaxPanel>
                                                                    </div>
                                                                    <div class="cf"></div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div style="clear: both;"></div>
                                                    </div>
                                                </div>
                                            </div>
                                        </li>

                                        <%--Custom Start--%>
                                        <li runat ="server" id="divCustom">
                                            <div id="accrdcustom" class="collapsible-header accrd accordian-text-custom ">
                                                <i class="mdi-action-account-circle"></i>
                                                <asp:Label ID="Label4" runat="server">Custom</asp:Label>
                                            </div>
                                            <div class="collapsible-body" id="dvCustomUser">
                                                <asp:HiddenField ID="hdnLineOpenned" runat="server" />
                                                <asp:HiddenField ID="hdnOrgMemberKey" runat="server" />
                                                <asp:HiddenField ID="hdnOrgMemberDisp" runat="server" />
                                                <asp:Panel ID="Panel1" runat="server">
                                                    <div class="form-content-wrap">
                                                        <div class="form-content-pd">
                                                            <div class="grid_container">
                                                                <div class="RadGrid RadGrid_Material FormGrid">
                                                                    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Setup" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                                        <telerik:RadGrid RenderMode="Auto" ID="gvUserCustom" ShowFooter="false" PageSize="50"
                                                                            PagerStyle-AlwaysVisible="false" OnItemDataBound="gvUserCustom_ItemDataBound" OnItemCommand="gvCustom_RowCommand"
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

                                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="140" AllowFiltering="false" HeaderText="Name" HeaderStyle-CssClass="itemHeader">
                                                                                        <ItemTemplate>
                                                                                            <div style="text-align: left ;">
                                                                                                <asp:HiddenField ID="hdnUserItemValueID" Value='' runat="server"></asp:HiddenField>
                                                                                                <asp:Panel ID="divFormatText" runat="server" Visible="false">
                                                                                                    <label class="fontHeader" for="txtFormatText"><%# Eval("Label") %></label>
                                                                                                    <asp:TextBox ID="txtFormatText" runat="server" Text='' style="width:95%"></asp:TextBox>
                                                                                                </asp:Panel>
                                                                                                <asp:Panel ID="divFormatDrop" runat="server" Visible="false">
                                                                                                    <label class="fontHeader" for="drpdwnCustom"><%# Eval("Label") %></label>
                                                                                                    <asp:DropDownList ID="drpdwnCustom" runat="server" CssClass="browser-default fontHeader" style="width:95%">
                                                                                                    </asp:DropDownList>
                                                                                                </asp:Panel>
                                                                                                <asp:Panel ID="divFormatCurrent" runat="server" Visible="false">
                                                                                                    <label class="fontHeader"  for="txtFormatCurrent"><%# Eval("Label") %></label>
                                                                                                    <asp:TextBox ID="txtFormatCurrent" runat="server" Text=''
                                                                                                        CssClass="custom currency" style="width:95%"></asp:TextBox>
                                                                                                </asp:Panel>
                                                                                                <asp:Panel ID="divFormatDate" runat="server" Visible="false">
                                                                                                    <label class="fontHeader"  for="txtFormatDate"><%# Eval("Label") %></label>
                                                                                                    <asp:TextBox ID="txtFormatDate" runat="server" Text=''
                                                                                                        CssClass="custom datepicker_mom" style="width:95%"></asp:TextBox>
                                                                                                </asp:Panel>
                                                                                                <asp:Panel ID="divFormatCheckbox" runat="server" Visible="false" >
                                                                                                    <asp:CheckBox ID="chkCustomFormat" runat="server" Text='&nbsp'
                                                                                                        CssClass="css-checkbox custom" style="width:95%"></asp:CheckBox>
                                                                                                    <label  class="fontHeader" for="chkCustomFormat"><%# Eval("Label") %></label>
                                                                                                </asp:Panel>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                        <FooterStyle VerticalAlign="Middle" />
                                                                                    </telerik:GridTemplateColumn>

                                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="50" AllowFiltering="false" HeaderText="Alert " HeaderStyle-CssClass="itemHeader">
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="chkSelectAlert" CssClass="css-checkbox" Text=" " runat="server" />
                                                                                        </ItemTemplate>
                                                                                        <FooterStyle VerticalAlign="Middle" />
                                                                                    </telerik:GridTemplateColumn>

                                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="120" AllowFiltering="false" HeaderText="Team Member" HeaderStyle-CssClass="itemHeader">
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

                                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="100" AllowFiltering="false" HeaderText="Updated Date" HeaderStyle-CssClass="itemHeader">
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
                                        <%--Custom End--%>

                                        <%--Documents Start--%>
                                        <li id="tbDocuments" runat="server">
                                            <div id="accrdDocuments" class="collapsible-header accrd accordian-text-custom"><i class="mdi-file-attachment"></i>Documents</div>
                                            <div class="collapsible-body">
                                                <asp:Panel ID="pnlDocPermission" runat="server">
                                                    <div class="form-section-row">
                                                        <div class="col s12 m12 l12">
                                                            <div class="row">
                                                                <asp:FileUpload ID="FileUpload1" runat="server" class="dropify" AllowMultiple="true" onchange="AddDocumentClick(this);" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="btncontainer">
                                                        <asp:Panel ID="pnlDocumentButtons" runat="server">
                                                            <div class="btnlinks">
                                                                <asp:LinkButton ID="lnkDeleteDoc" runat="server" CausesValidation="False" OnClick="lnkDeleteDoc_Click" OnClientClick="return DeleteDocumentClick(this);">Delete</asp:LinkButton>
                                                                <asp:LinkButton ID="lnkUploadDoc" runat="server" CausesValidation="False" OnClick="lnkUploadDoc_Click" Style="display: none">Upload</asp:LinkButton>
                                                                <asp:LinkButton ID="lnkPostback" runat="server" CausesValidation="False" Style="display: none">Postback</asp:LinkButton>
                                                            </div>
                                                        </asp:Panel>
                                                        <div style="clear: both;"></div>
                                                    </div>
                                                    <div class="form-section-row">
                                                        <div class="grid_container">
                                                            <div class="RadGrid RadGrid_Material FormGrid">
                                                                <telerik:RadCodeBlock ID="RadCodeBlock_Documents" runat="server">
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

                                                                        function requestStart2(sender, args) {
                                                                            requestInitiator = document.activeElement.id;
                                                                            if (document.activeElement.tagName == "INPUT") {
                                                                                selectionStart = document.activeElement.selectionStart;
                                                                            }


                                                                        }

                                                                        function responseEnd2(sender, args) {
                                                                            try {
                                                                                var element = document.getElementById(requestInitiator);
                                                                                if (element && element.tagName == "INPUT") {
                                                                                    element.focus();
                                                                                    element.selectionStart = selectionStart;
                                                                                }

                                                                            } catch (e) {

                                                                            }
                                                                        }
                                                                    </script>

                                                                </telerik:RadCodeBlock>

                                                                <telerik:RadAjaxPanel ID="RadAjaxPanel_Documents" PostBackControls="lblName" runat="server" ClientEvents-OnRequestStart="requestStart2" ClientEvents-OnResponseEnd="responseEnd2">
                                                                    <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid_Documents" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                                        PagerStyle-AlwaysVisible="true" OnPreRender="RadGrid_Documents_PreRender" OnNeedDataSource="RadGrid_Documents_NeedDataSource"
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

                                                                                <telerik:GridTemplateColumn AllowFiltering="false" Visible="false" ShowFilterIcon="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblID" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                                                                        <asp:HiddenField runat="server" ID="hdnTempId" Value='<%# Eval("id").ToString() == "0"? Eval("TempId"): string.Empty %>' />
                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>

                                                                                <telerik:GridTemplateColumn DataField="filename" SortExpression="filename" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                                                    HeaderText="File Name" ShowFilterIcon="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:LinkButton ID="lblName" runat="server" CausesValidation="false"
                                                                                            CommandArgument='<%#Eval("filename") + "," + Eval("Path") %>'
                                                                                            OnClientClick="return ViewDocumentClick(this);" OnClick="lblName_Click" Text='<%# Eval("filename") %>'>
                                                                                        </asp:LinkButton>
                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>

                                                                                <telerik:GridBoundColumn FilterDelay="5" DataField="doctype" HeaderText="File Type" HeaderStyle-Width="140"
                                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="doctype"
                                                                                    ShowFilterIcon="false">
                                                                                </telerik:GridBoundColumn>

                                                                                <%--<telerik:GridTemplateColumn SortExpression="portal" HeaderText="Portal" DataField="portal" ShowFilterIcon="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="chkPortal" runat="server" Checked='<%# (Eval("portal")!=DBNull.Value) ? Convert.ToBoolean(Eval("portal")): false %>' />
                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>--%>

                                                                                <%--<telerik:GridTemplateColumn DataField="MSVisible" SortExpression="MSVisible" AutoPostBackOnFilter="true"
                                                                                    CurrentFilterFunction="Contains" HeaderText="Mobile Service" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="chkMSVisible" runat="server" Checked='<%# (Eval("MSVisible")!=DBNull.Value) ? Convert.ToBoolean(Eval("MSVisible")): false %>' />
                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>--%>

                                                                                <telerik:GridTemplateColumn SortExpression="remarks" HeaderText="Remarks" DataField="remarks" ShowFilterIcon="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtremarks" runat="server" Text='<%# Eval("remarks") %>'></asp:TextBox>
                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>
                                                                            </Columns>
                                                                        </MasterTableView>
                                                                    </telerik:RadGrid>
                                                                </telerik:RadAjaxPanel>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                                <div style="clear: both;"></div>
                                            </div>
                                        </li>
                                        <%--Documents End--%>

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
                                                                                        <asp:Label ID="lbldate" runat="server" Text='<%# Eval("CreatedStamp").ToString() != "" ? Eval("CreatedStamp", "{0:M/d/yyyy}") : Eval("fDate", "{0:M/d/yyyy}")%>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </telerik:GridTemplateColumn>
                                                                                <telerik:GridTemplateColumn DataField="CreatedStamp" SortExpression="CreatedStamp" AutoPostBackOnFilter="true" DataType="System.String"
                                                                                    CurrentFilterFunction="Contains" HeaderText="Time" ShowFilterIcon="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lbltime" runat="server" Text='<%# Eval("CreatedStamp").ToString() != "" ? Eval("CreatedStamp","{0: hh:mm tt}") : Eval("fTime", "{0: hh:mm tt}") %>'></asp:Label>
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
                </div>
            </div>
        </div>
    </div>




    <div class="container accordian-wrap" style="display: none;">
        <div class="row">
            <div class="col s12 m12 l12">
                <div class="row">
                    <div class="card" style="min-height: 700px !important; border-radius: 6px;">
                        <div class="card-content">
                            <div class="form-content-wrap">
                                <div class="form-content-pd">
                                    <div class="form-section-row">
                                        <div class="row">
                                            <div class="col s12 m12 l12">
                                                <div class="row">
                                                    <div class="col s12 m12 l12">
                                                        <ul class="tabs tab-demo-active white" style="width: 100%;">
                                                            <li class="tab col s2">
                                                                <a class="waves-effect waves-light active" href="#activeone"><i class="mdi-maps-local-library"></i>&nbsp;Vendor Info</a>
                                                            </li>
                                                            <li class="tab col s2">
                                                                <a class="waves-effect waves-light" href="#two"><i class="mdi-editor-attach-money"></i>&nbsp;Transactions</a>
                                                            </li>

                                                        </ul>
                                                    </div>
                                                    <div class="col s12">
                                                        <div id="activeone" class="col s12 tab-container-border lighten-4" style="display: block;">
                                                            <div class="form-content-wrap" style="overflow: auto; margin-top: 20px; padding-top: 5px;">
                                                                <div class="tabs-custom-mgn1 mn-ht">
                                                                    <div class="form-content-wrap">
                                                                    </div>

                                                                </div>
                                                            </div>

                                                        </div>
                                                        <div id="two" class="col s12 tab-container-border lighten-4" style="display: none;">
                                                            <div class="form-content-wrap" style="overflow: auto; margin-top: 20px; padding-top: 5px;">
                                                                <div class="tabs-custom-mgn1 mn-ht">
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
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
        </div>
    </div>
    <telerik:RadWindowManager ID="RadWindowManager" runat="server" EnableShadow="true" VisibleStatusbar="false" CssClass="headerCollection">
        <Windows>
            <telerik:RadWindow ID="EmailSignatureWindow" Skin="Material" VisibleTitlebar="true" Title="Email Signature" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="300"
                runat="server" Modal="true" Width="800" ShowContentDuringLoad="false" Height="530">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

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


    <asp:HiddenField ID="hdnApplyUserRolePermissionOrg" runat="server" />
    <asp:HiddenField ID="hdnUserRoleOrg" runat="server" />

    <!-- Hidden Field -->
    <asp:HiddenField runat="server" ID="hdnAddeDocument" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditDocument" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeleteDocument" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewDocument" Value="Y" />

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
            //debugger
            var apprPO = $("#<%=ddlPOApprove.ClientID%>").val();
            HideShowPOAmount(apprPO);
        });
        function CopyToBCC() {
            $("#<%=txtBccEmail.ClientID%>").val($("#<%=txtInUSername.ClientID%>").val());
            $("#<%=txtBccEmail.ClientID%>").focus();
        }

        function LoadLogs() {
            if (document.getElementById('<%= hdnLoadLogs.ClientID%>').value == "0") {
                document.getElementById('<%= lnkLoadLogs.ClientID%>').click();
            }
        }

        function ApplyUserRolePermissionChange(ee) {
            var estimateMode = $(ee).val();
            if (estimateMode == "2") {
                return noty({
                    dismissQueue: true,
                    layout: 'topCenter',
                    theme: 'noty_theme_default',
                    animateOpen: { height: 'toggle' },
                    animateClose: { height: 'toggle' },
                    easing: 'swing',
                    text: 'Are you sure want to override user\'s permissions by user role\'s permissions?',
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
                                __doPostBack("ctl00$ContentPlaceHolder1$ddlApplyUserRolePermission", "ddlApplyUserRolePermissionchange");
                                return true;
                            }
                        },
                        {
                            type: 'btn-danger', text: 'No', click: function ($noty) {

                                $noty.close();
                                var aa = $("#<%=hdnApplyUserRolePermissionOrg.ClientID%>").val();
                                $(ee).val(aa);
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
            }
            else if (estimateMode == "1") {
                return noty({
                    dismissQueue: true,
                    layout: 'topCenter',
                    theme: 'noty_theme_default',
                    animateOpen: { height: 'toggle' },
                    animateClose: { height: 'toggle' },
                    easing: 'swing',
                    text: 'Are you sure want to merge user\'s permissions and user role\'s permissions?',
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
                                __doPostBack("ctl00$ContentPlaceHolder1$ddlApplyUserRolePermission", "ddlApplyUserRolePermissionchange");
                                return true;
                            }
                        },
                        {
                            type: 'btn-danger', text: 'No', click: function ($noty) {

                                $noty.close();
                                var aa = $("#<%=hdnApplyUserRolePermissionOrg.ClientID%>").val();
                                $(ee).val(aa);
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
            }
            else {
                __doPostBack("ctl00$ContentPlaceHolder1$ddlApplyUserRolePermission", "ddlApplyUserRolePermissionchange");
                return true;
            }
        }

        function UserRoleChange(ee) {
            var estimateMode = $(ee).val();

            if (estimateMode != "0") {
                $("#<%=hdnUserRoleOrg.ClientID%>").val(estimateMode);
                $("#<%=divApplyUserRolePermission.ClientID%>").css("display", "block");
                return noty({
                    dismissQueue: true,
                    layout: 'topCenter',
                    theme: 'noty_theme_default',
                    animateOpen: { height: 'toggle' },
                    animateClose: { height: 'toggle' },
                    easing: 'swing',
                    text: 'You are changing the user role of the user.  Do you want to Merge/Override the user permissions?',
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
                            type: 'btn-primary', text: 'Merge', click: function ($noty) {
                                $noty.close();
                                $('#<%=ddlApplyUserRolePermission.ClientID%>').val("1");
                                <%--$("#<%=hdnApplyUserRolePermissionOrg.ClientID%>").val("1");
                                __doPostBack("ctl00$ContentPlaceHolder1$ddlUserRole", "ddlUserRolechange");
                                return true;--%>
                                return noty({
                                    dismissQueue: true,
                                    layout: 'topCenter',
                                    theme: 'noty_theme_default',
                                    animateOpen: { height: 'toggle' },
                                    animateClose: { height: 'toggle' },
                                    easing: 'swing',
                                    text: 'Are you sure want to merge user\'s permissions with user role\'s permissions?',
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
                                                debugger
                                                <%--$("#<%=hdnApplyUserRolePermissionOrg.ClientID%>").val("1");--%>
                                                <%--$('#<%=ddlApplyUserRolePermission.ClientID%>').val("1");--%>
                                                __doPostBack("ctl00$ContentPlaceHolder1$ddlApplyUserRolePermission", "ddlApplyUserRolePermissionchange");
                                                return true;
                                            }
                                        },
                                        {
                                            type: 'btn-danger', text: 'No', click: function ($noty) {

                                                $noty.close();
                                                <%--$("#<%=hdnApplyUserRolePermissionOrg.ClientID%>").val("0");--%>
                                                $('#<%=ddlApplyUserRolePermission.ClientID%>').val("0");
                                                //return false;
                                                __doPostBack("ctl00$ContentPlaceHolder1$ddlUserRole", "ddlUserRolechange");
                                                return true;
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

                                return false;
                            }
                        },
                        {
                            type: 'btn-danger', text: 'Override', click: function ($noty) {

                                $noty.close();
                                $('#<%=ddlApplyUserRolePermission.ClientID%>').val("2");

                                <%--var aa = $("#<%=hdnApplyUserRolePermissionOrg.ClientID%>").val();
                                $(ee).val(aa);--%>
                                //__doPostBack("ctl00$ContentPlaceHolder1$ddlUserRole", "ddlUserRolechange");
                                return noty({
                                    dismissQueue: true,
                                    layout: 'topCenter',
                                    theme: 'noty_theme_default',
                                    animateOpen: { height: 'toggle' },
                                    animateClose: { height: 'toggle' },
                                    easing: 'swing',
                                    text: 'Are you sure want to override user\'s permissions by user role\'s permissions?',
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
                                                <%--$("#<%=hdnApplyUserRolePermissionOrg.ClientID%>").val("2");
                                                $('#<%=ddlApplyUserRolePermission.ClientID%>').val("2");--%>
                                                __doPostBack("ctl00$ContentPlaceHolder1$ddlApplyUserRolePermission", "ddlApplyUserRolePermissionchange");
                                                return true;
                                            }
                                        },
                                        {
                                            type: 'btn-danger', text: 'No', click: function ($noty) {

                                                $noty.close();
                                                $("#<%=hdnApplyUserRolePermissionOrg.ClientID%>").val("0");
                                                $('#<%=ddlApplyUserRolePermission.ClientID%>').val("0");
                                                //return false;
                                                __doPostBack("ctl00$ContentPlaceHolder1$ddlUserRole", "ddlUserRolechange");
                                                return true;
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

                                return false;
                            }
                        }
                        ,
                        {
                            type: 'btn-danger', text: 'None', click: function ($noty) {

                                $noty.close();
                                <%--var aa = $("#<%=hdnApplyUserRolePermissionOrg.ClientID%>").val();
                                $(ee).val(aa);--%>
                                debugger
                                $('#<%=ddlApplyUserRolePermission.ClientID%>').val("0");
                                <%--$("#<%=hdnApplyUserRolePermissionOrg.ClientID%>").val("0");--%>
                                __doPostBack("ctl00$ContentPlaceHolder1$ddlUserRole", "ddlUserRolechange");
                                return true;
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
                <%--return noty({
                    dismissQueue: true,
                    layout: 'topCenter',
                    theme: 'noty_theme_default',
                    animateOpen: { height: 'toggle' },
                    animateClose: { height: 'toggle' },
                    easing: 'swing',
                    text: 'Permissions will not be changed on removing user role.  Do you want to continue?',
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
                                $('#<%=ddlApplyUserRolePermission.ClientID%>').val("0");
                                $("#<%=hdnUserRoleOrg.ClientID%>").val("0");
                                $("#<%=divApplyUserRolePermission.ClientID%>").css("display", "none");
                                __doPostBack("ctl00$ContentPlaceHolder1$ddlUserRole", "ddlUserRolechange");
                                return true;
                            }
                        },
                        {
                            type: 'btn-danger', text: 'No', click: function ($noty) {

                                $noty.close();
                                var preVal = $("#<%=hdnUserRoleOrg.ClientID%>").val();
                                $('#<%=ddlUserRole.ClientID%>').val(preVal);
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
                
                return false;--%>

                $("#<%=divApplyUserRolePermission.ClientID%>").css("display", "none");
                $('#<%=ddlApplyUserRolePermission.ClientID%>').val("0");
                noty({ text: 'Permissions will not be changed on removing user role', dismissQueue: true, type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: true });
                __doPostBack("ctl00$ContentPlaceHolder1$ddlUserRole", "ddlUserRolechange");
                return true;
            }
        }

        //// Permission Check BOX
        $(document).ready(function () {

            $("#<%=chkBillingmodule.ClientID%>").click(function () {
                if (!$(this).is(':checked')) {
                    // Do stuff 
                    $("#<%=chkInvoicesAdd.ClientID%>").prop('checked', false);
                    $("#<%=chkInvoicesEdit.ClientID%>").prop('checked', false);
                    $("#<%=chkInvoicesDelete.ClientID%>").prop('checked', false);
                    $("#<%=chkInvoicesView.ClientID%>").prop('checked', false);

                    $("#<%=chkBillingcodesAdd.ClientID%>").prop('checked', false);
                    $("#<%=chkBillingcodesEdit.ClientID%>").prop('checked', false);
                    $("#<%=chkBillingcodesDelete.ClientID%>").prop('checked', false);
                    $("#<%=chkBillingcodesView.ClientID%>").prop('checked', false);
                }
            });

            $("#<%=chkPurchasingmodule.ClientID%>").click(function () {
                if (!$(this).is(':checked')) {
                    // Do stuff 
                    $("#<%=chkPOAdd.ClientID%>").prop('checked', false);
                    $("#<%=chkPOEdit.ClientID%>").prop('checked', false);
                    $("#<%=chkPODelete.ClientID%>").prop('checked', false);
                    $("#<%=chkPOView.ClientID%>").prop('checked', false);
                }
            });

            $("#<%=chkPOView.ClientID%>").click(function () {
                if (!$(this).is(':checked')) {
                    // Do stuff 
                    $("#<%=chkPOAdd.ClientID%>").prop('checked', false);
                    $("#<%=chkPOEdit.ClientID%>").prop('checked', false);
                    $("#<%=chkPODelete.ClientID%>").prop('checked', false);
                }
            });

            $("#<%=chkBillingcodesView.ClientID%>").click(function () {
                if (!$(this).is(':checked')) {
                    // Do stuff 
                    $("#<%=chkBillingcodesAdd.ClientID%>").prop('checked', false);
                    $("#<%=chkBillingcodesEdit.ClientID%>").prop('checked', false);
                    $("#<%=chkBillingcodesDelete.ClientID%>").prop('checked', false);
                }
            });

            $("#<%=chkInvoicesView.ClientID%>").click(function () {
                if (!$(this).is(':checked')) {
                    // Do stuff 
                    $("#<%=chkInvoicesAdd.ClientID%>").prop('checked', false);
                    $("#<%=chkInvoicesEdit.ClientID%>").prop('checked', false);
                    $("#<%=chkInvoicesDelete.ClientID%>").prop('checked', false);
                }
            });

            var selectedUserRole = $("#<%=ddlUserRole.ClientID%>").val();
            if (selectedUserRole == "0") {
                $("#<%=divApplyUserRolePermission.ClientID%>").css("display", "none");
            } else {
                $("#<%=divApplyUserRolePermission.ClientID%>").css("display", "block");
            }

            Materialize.updateTextFields();

        });

        function ShowHideApprovePOInfo() {
            var isApprovePO = $("#<%=ddlPOApprove.ClientID%>").val();
            if (isApprovePO == "1") {
                $("#<%=divApprovePo.ClientID%>").css("display", "block");

                var seletedPOApproveAmt = $("#<%=ddlPOApproveAmt.ClientID%>").val();
                if (seletedPOApproveAmt == "0") {
                    $("#<%=divMinAmount.ClientID%>").css("display", "block");
                    $("#<%=divMaxAmount.ClientID%>").css("display", "block");
                } else if (seletedPOApproveAmt == "1") {
                    $("#<%=divMinAmount.ClientID%>").css("display", "block");
                    $("#<%=divMaxAmount.ClientID%>").css("display", "none");
                } else {
                    $("#<%=divMinAmount.ClientID%>").css("display", "none");
                    $("#<%=divMaxAmount.ClientID%>").css("display", "none");
                }
            } else {
                $("#<%=divApprovePo.ClientID%>").css("display", "none");
                $("#<%=divMinAmount.ClientID%>").css("display", "none");
                $("#<%=divMaxAmount.ClientID%>").css("display", "none");
            }
        }

            function pageLoad() {



                /////////////////////////////////// Wage ////////////////////////////////////

                $("[id*=txtWage]").autocomplete({
                    source: function (request, response) { 

                        {

                            var dtaaa = new dtaa();
                            dtaaa.prefixText = request.term;
                            dtaaa.con = "";
                            query = request.term;
                            $.ajax({
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                url: "AccountAutoFill.asmx/GetWageUserScreen",
                                data: JSON.stringify(dtaaa),
                                dataType: "json",
                                async: true,
                                success: function (data) {
                                    response($.parseJSON(data.d));
                                },
                                error: function (result) {
                                    alert("Due to unexpected errors we were unable to load Wage name");
                                }
                            });


                        }
                    },
                    select: function (event, ui) {
                        debugger;
                        var txtWage = this.id;
                        var hdnWageID = document.getElementById(txtWage.replace('txtWage', 'hdnWageID'));                     
                        var hdnWageIDtext = document.getElementById(txtWage.replace('txtWage', 'hdnWageIDtext')).value; 
                        var hdnIsUsed = document.getElementById(txtWage.replace('txtWage', 'hdnIsUsed')).value; 
                        var txtReg =    document.getElementById(txtWage.replace('txtWage', 'txtReg'));
                        var txtOt =     document.getElementById(txtWage.replace('txtWage', 'txtOt'));
                        var txtNt =     document.getElementById(txtWage.replace('txtWage', 'txtNt'));
                        var txtDt =     document.getElementById(txtWage.replace('txtWage', 'txtDt'));
                        var txttt = document.getElementById(txtWage.replace('txtWage', 'txtTt'));

                        var txtCReg =   document.getElementById(txtWage.replace('txtWage', 'txtCReg'));
                        var txtCOt =    document.getElementById(txtWage.replace('txtWage', 'txtCOt'));
                        var txtCNt =    document.getElementById(txtWage.replace('txtWage', 'txtCNt'));
                        var txtCDt =    document.getElementById(txtWage.replace('txtWage', 'txtCDt'));
                        var txtCTt =    document.getElementById(txtWage.replace('txtWage', 'txtCTt'));   
         
                        
                        
                        var str = ui.item.NAME;
                        var strId = ui.item.ID;
                        var strtxtReg = ui.item.RT;
                        var strtxtOt = ui.item.OT;
                        var strtxtNt = ui.item.NT;
                        var strtxtDt = ui.item.DT; 
                        var strtxttt = ui.item.TT; 

                        var strtxtCReg = ui.item.CRT;
                        var strtxtCOt = ui.item.COT;
                        var strtxtCNt = ui.item.CNT;
                        var strtxtCDt = ui.item.CDT;
                        var strtxtCTt = ui.item.CTT; 


                      

                        if (str == "No Record Found!") {
                            $(txtWage).val("");
                        }
                        else if (hdnIsUsed == '1') {
                            $(this).val(hdnWageIDtext); 
                        }
                        else {
                            $(this).val(str);
                            $(hdnWageID).val(strId);
                            $(txtReg).val(strtxtReg);
                            $(txtOt).val(strtxtOt);
                            $(txtNt).val(strtxtNt);
                            $(txtDt).val(strtxtDt);
                            $(txttt).val(strtxttt);


                            $(txtCReg).val(strtxtCReg);
                            $(txtCOt).val(strtxtCOt);
                            $(txtCNt).val(strtxtCNt);
                            $(txtCDt).val(strtxtCDt);
                            $(txtCTt).val(strtxtCTt);

                           
                        }
                        return false;
                    },
                    focus: function (event, ui) {
                        var txtWage = this.ID;
                        $(txtWage).val(ui.item.NAME);
                        return false;
                    },
                    minLength: 0,
                    delay: 50
                }).bind('click', function () { $(this).autocomplete("search"); })
                $.each($(".wage-search"), function (index, item) {
                    $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
                        var ula = ul;
                        var itema = item;
                        var result_value = item.ID;
                        var result_item = item.NAME;
                        //var result_desc = item.acct;
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
                                .append("<a>" + result_item + "</a>")
                                .appendTo(ul);
                        }
                    };
                });

            Materialize.updateTextFields();
        }
    </script>
    <script>
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


        /* Stage for Project*/

        function OpenEditEmailSignatureWindow() {
            var RefID = "";
            $("#<%= RadGrid_EmailSigns.ClientID %>").find('tr:not(:first,:last)').each(function () {
                var $tr = $(this);
                if ($tr.find('input[type="checkbox"]').prop("checked") == true) {
                    var val = $tr.find('input[id*=hdnSignId]').val();
                    RefID = val;
                }
            });
            var userId = '<%= Request.QueryString["uid"] != null ? Request.QueryString["uid"].ToString() : "" %>';
            var strUrl = "EmailSignaturePopup.aspx?id=" + RefID +"&userId=" + userId;
            var oWnd = radopen(strUrl, "<%=EmailSignatureWindow.ClientID%>"); //Pass parameter using URL 

            oWnd.setSize(800, 530);
            oWnd.minimize();
            oWnd.maximize();
            oWnd.restore();
            oWnd.center();
            oWnd.set_modal(true);
            oWnd.show();
        }

        function OpenAddEmailSignatureWindow() {
            var userId = '<%= Request.QueryString["uid"] != null ? Request.QueryString["uid"].ToString() : "" %>';
            var strUrl = "EmailSignaturePopup.aspx?userId=" + userId;
            var oWnd = radopen(strUrl, "<%=EmailSignatureWindow.ClientID%>"); //Pass parameter using URL 

            oWnd.setSize(800, 530);
            //oWnd.minimize();
            //oWnd.maximize();
            oWnd.restore();
            oWnd.center();
            oWnd.set_modal(true);
            oWnd.show();
        }
        <%--

        function CloseStageProjectWindow(msg) {
            var wnd = "";

            if (msg == 'Add') {
                wnd = $find('<%=StageProjectWindow.ClientID %>');
            }
            else {
                var HeaderText = document.getElementById('<%= txtEditStageProject.ClientID %>').value;
                document.getElementById('<%= lnkEditStageProjectHeader.ClientID %>').innerText = HeaderText;
                document.getElementById('<%= lblEditStageProject.ClientID %>').innerText = HeaderText;
                document.getElementById('<%= lblStageProjectHeader.ClientID %>').innerText = HeaderText;
                var wnd = $find('<%=StageProjectlabelWindow.ClientID %>');
            }
            wnd.Close();
        }--%>
        /* Stage for Project*/
    </script>

    <script type="text/javascript">
        ///-Document permission

        function AddDocumentClick(hyperlink) {

            var IsAdd = document.getElementById('<%= hdnAddeDocument.ClientID%>').value;
            if (IsAdd == "Y") {
                debugger;
                ConfirmUpload(ctl00_ContentPlaceHolder1_FileUpload1.value);
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
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

        function ViewDocumentClick(hyperlink) {
            var IsView = document.getElementById('<%= hdnViewDocument.ClientID%>').value;
            if (IsView == "Y") {
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }


        function checkdelete() {
            return SelectedRowDelete('<%= RadGrid_Documents.ClientID %>', 'file');
        }

        function SelectedRowDelete(gridview, message) {
            var grid = $find(gridview);
            var MasterTable = grid.get_masterTableView();
            var Rows = null;
            if (MasterTable != null) {
                Rows = MasterTable.get_dataItems();
            }
            if (Rows != null && Rows.length > 0) {
                for (i = 0; i < Rows.length; i++) {
                    if (Rows[i].get_selected()) {
                        //return confirm('Are you sure you want to delete ' + message + ' "' + Rows[i].get_columns(1) + '" ?');
                        return true;
                    }
                }
            }
            alert('Please select ' + message + '.');
            return false;
        }

        ////////////////// Confirm Document Upload ////////////////////
        <%--function ConfirmUpload(value) {
            //debugger
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
        }--%>

        function ConfirmUpload(value) {
            if (confirm('Are you sure you want to upload?')) { document.getElementById('<%= lnkUploadDoc.ClientID %>').click(); }
            else { document.getElementById('<%= lnkPostback.ClientID %>').click(); }
        }

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

            Materialize.updateTextFields();
        });
    </script>

</asp:Content>
