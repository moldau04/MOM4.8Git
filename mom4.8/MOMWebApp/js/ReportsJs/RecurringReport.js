


$(function () {
    $("#MoveRight,#MoveLeft").click(function (event) {
        var id = $(event.target).attr("id");
        var selectFrom = id == "MoveRight" ? "#from" : "#to";
        var moveTo = id == "MoveRight" ? "#to" : "#from";

        var selectedItems = $(selectFrom + " :selected").toArray();
        $(moveTo).append(selectedItems);
        selectedItems.remove;
    });
});
//function moveUp() {
$(function () {
    $("#MoveUp").click(function (event) {
        $('#lstColumnSort :selected').each(function (i, selected) {
            if (!$(this).prev().length) return false;
            $(this).insertBefore($(this).prev());
        });
        $('#lstColumnSort select').focus().blur();

    });
});

//function moveDown() {
$(function () {
    $("#MoveDown").click(function (event) {
        $($('#lstColumnSort :selected').get().reverse()).each(function (i, selected) {
            if (!$(this).next().length) return false;
            $(this).insertAfter($(this).next());
        });
        $('#lstColumnSort select').focus().blur();

    });
});
$(function () {
    $("#ReadAll").click(function (event) {
        $('#lstColumnSort option').each(function (index) {
            //  if ( ($(this).is(':selected')) {
            //    // do stuff if selected
            //  }
            //  else {
            //   // this one isn't selected, do other stuff
            //  }
            alert($(this).val() + '--' + $(this).text());
        });
    });
});
$(function () {

    $("#Delete").click(function (event) {
        $('#lstColumnSort :selected').each(function (i, selected) {

            $(this).remove();
            alert('Selected item deleted');
            $('#lstColumnSort').focus().blur();

        });
    });
});


function UserDeleteConfirmation() {
    return confirm("Are you sure you want to delete this report?");
}

function EmptyReportName() {
    debugger;
    var _reportName = $('#txtReportName').val();
    var _hdnName = $("#hdnCustomizeReportName").val();
    var _isStock = $("#hdnIsStock").val();

    if ($("#txtReportName").val() == "") {
        alert("Report name cann't be empty.");
        return false;
    }
    else {
        $("#divInfo").hide();
        if (_reportName != _hdnName && _isStock == "True") {
            $("#hdnReportAction").val('Save');
            return true;
        }
        else if (_reportName == _hdnName && _isStock == "True") {
            alert("You don't have permission to update this report. Please choose another title.");
            $("#divInfo").css('display', 'block');
            return false;
        }
        else {
            //$("#hdnReportAction").val('Save');
            return true;
        }
    }
}

//function EmptyEmailBox() {
//    if ($("#txtEmails").val() == "") {
//        alert("Report name cann't be empty.");
//        return false;
//    }
//    else {
//        return true;
//    }
//}

function EmptyFilters() {
    $("#tblName").css("display", "none");
    //$("#drpName").val('All');
    $('#drpName input[type="checkbox"]').attr('checked', false);
    $("#tblCity").css("display", "none");
    //            $("#txtCity").val('');
    $('#drpCity input[type="checkbox"]').attr('checked', false);
    $("#tblState").css("display", "none");

    //Changed by Yashasvi Jadav
    $("#tblRoute").css("display", "none");
    $('#drpRoute input[type="checkbox"]').attr('checked', false);
    $("#tblLocationSTax").css("display", "none");
    $('#drpLocationSTax input[type="checkbox"]').attr('checked', false);
    $("#tblDefaultSalesPerson").css("display", "none");
    $('#drpDefaultSalesPerson input[type="checkbox"]').attr('checked', false);
    $("#tblInstalledOn").css("display", "none");
    $("#txtInstalledOn").val('');
    $("#drpBuldingType").css("display", "none");
    $('#drpBuldingType input[type="checkbox"]').attr('checked', false);
    $("#drpEquipmentState").css("display", "none");
    $('#drpEquipmentState input[type="checkbox"]').attr('checked', false);
    //$("#ddlState").val('All');
    $('#ddlState input[type="checkbox"]').attr('checked', false);
    $("#tblZip").css("display", "none");
    $("#txtZip").val('');
    $("#txtInstalledOn").val('');
    $("#tblPhone").css("display", "none");
    $("#txtPhone").val('');
    $("#tblFax").css("display", "none");
    $("#txtFax").val('');
    $("#tblContact").css("display", "none");
    $("#txtContact").val('');
    $("#tblAddress").css("display", "none");
    //            $("#txtAddress").val('');
    $('#drpAddress input[type="checkbox"]').attr('checked', false);
    $("#tblEmail").css("display", "none");
    $("#txtEmail").val('');
    $("#tblCountry").css("display", "none");
    $("#txtCountry").val('');
    $("#tblWebsite").css("display", "none");
    $("#txtWebsite").val('');
    $("#tblCellular").css("display", "none");
    $("#txtCellular").val('');
    $("#tblCategory").css("display", "none");
    $("#drpCategory").val('Category');
    $("#tblType").css("display", "none");
    // $("#drpType").val('All');
    $('#drpType input[type="checkbox"]').attr('checked', false);

    $("#tblBalance").css("display", "none");
    $("#txtBalEqual").val('');
    $("#txtBalLessAndEqual").val('');
    $("#txtBalGreaterAndEqual").val('');

    $("#tblStatus").css("display", "none");
    $("#drpStatus").val('All');


    $("#tblCustomer").css("display", "none");
    $('#drpCustomer input[type="checkbox"]').attr('checked', false);

    $("#tblLocationId").css("display", "none");
    $('#drpLocationId input[type="checkbox"]').attr('checked', false);

    $("#tblLocationName").css("display", "none");
    $('#drpLocationName input[type="checkbox"]').attr('checked', false);

    $("#tblLocType").css("display", "none");
    $('#drpLocationType input[type="checkbox"]').attr('checked', false);

    $("#tblServiceType").css("display", "none");
    $('#drpServiceType input[type="checkbox"]').attr('checked', false);

    $("#tblTicketStart").css("display", "none");
    $('#drpTicketStart input[type="checkbox"]').attr('checked', false);

    $("#tblTicketTime").css("display", "none");
    $('#drpTicketTime input[type="checkbox"]').attr('checked', false);

    $("#tblHours").css("display", "none");
    $('#drpHours input[type="checkbox"]').attr('checked', false);

    $("#tblBillFreqency").css("display", "none");
    $('#drpBillFreqency input[type="checkbox"]').attr('checked', false);

    $("#tblTicketFreq").css("display", "none");
    $('#drpTicketFreq input[type="checkbox"]').attr('checked', false);

    $("#tblPreferredWorker").css("display", "none");
    $('#drpPreferredWorker input[type="checkbox"]').attr('checked', false);

    $("#tblDescription").css("display", "none");
    $('#drpDescription input[type="checkbox"]').attr('checked', false);

    $("#tblBillAmount").css("display", "none");
    $('#drpBillAmount input[type="checkbox"]').attr('checked', false);

    $("#tblBillFreqency").css("display", "none");
    $('#drpBillFreqency input[type="checkbox"]').attr('checked', false);

    $("#tblEquipment").css("display", "none");
    $('#drpEquipment input[type="checkbox"]').attr('checked', false);

    $("#tblExpiration").css("display", "none");
    $('#drpExpiration input[type="checkbox"]').attr('checked', false);

    $("#tblExpirationDate").css("display", "none");
    $('#drpExpirationDate input[type="checkbox"]').attr('checked', false);

    $("#tblPhoneMonitoring").css("display", "none");
    $('#drpPhoneMonitoring input[type="checkbox"]').attr('checked', false);

    $("#tblContractType").css("display", "none");
    $('#drpContractType input[type="checkbox"]').attr('checked', false);

    $("#tblOccupancyDiscount").css("display", "none");
    $('#drpOccupancyDiscount input[type="checkbox"]').attr('checked', false);

    $("#tblExclusions").css("display", "none");
    $('#drpExclusions input[type="checkbox"]').attr('checked', false);

    $("#tblTermOfContract").css("display", "none");
    $('#drpTermOfContract input[type="checkbox"]').attr('checked', false);

    $("#tblPriceAdjustmentCap").css("display", "none");
    $('#drpPriceAdjustmentCap input[type="checkbox"]').attr('checked', false);

    $("#tblFireServiceTestingIncluded").css("display", "none");
    $('#drpFireServiceTestingIncluded input[type="checkbox"]').attr('checked', false);

    $("#tblSpecialRates").css("display", "none");
    $('#drpSpecialRates input[type="checkbox"]').attr('checked', false);

    $("#tblContractExpiration").css("display", "none");
    $('#drpContractExpiration input[type="checkbox"]').attr('checked', false);

    $("#tblProratedItems").css("display", "none");
    $('#drpProratedItems input[type="checkbox"]').attr('checked', false);

    $("#tblAnnualTestIncluded").css("display", "none");
    $('#drpAnnualTestIncluded input[type="checkbox"]').attr('checked', false);

    $("#tblFiveYearStateTestIncluded").css("display", "none");
    $('#drpFiveYearStateTestIncluded input[type="checkbox"]').attr('checked', false);

    $("#tblFireServiceTestedIncluded").css("display", "none");
    $('#drpFireServiceTestedIncluded input[type="checkbox"]').attr('checked', false);

    $("#tblCancellationNotificationDays").css("display", "none");
    $('#drpCancellationNotificationDays input[type="checkbox"]').attr('checked', false);

    $("#tblPriceAdjustmentNotificationDays").css("display", "none");
    $('#drpPriceAdjustmentNotificationDays input[type="checkbox"]').attr('checked', false);

    $("#tblAfterHoursCallsIncluded").css("display", "none");
    $('#drpAfterHoursCallsIncluded input[type="checkbox"]').attr('checked', false);

    $("#tblOGServiceCallsIncluded").css("display", "none");
    $('#drpOGServiceCallsIncluded input[type="checkbox"]').attr('checked', false);

    $("#tblContractHours").css("display", "none");
    $('#drpContractHours input[type="checkbox"]').attr('checked', false);

    $("#tblContractFormat").css("display", "none");
    $('#drpContractFormat input[type="checkbox"]').attr('checked', false);


    $("#tblLoc").css("display", "none");
    $("#txtLocEqual").val('');
    $("#txtLocLessAndEqual").val('');
    $("#txtLocGreatAndEqual").val('');

    $("#tblEquipmentCounts").css("display", "none");
    $("#txtEquipmentCountsEqual").val('');
    $("#txtEquipmentCountsLessAndEqual").val('');
    $("#txtEquipmentCountsGreatAndEqual").val('');

    //Added by Yashasvi Jadav
    $("#tblEquip").css("display", "none");
    $("#txtEquipEqual").val('');
    $("#txtEquipLessAndEqual").val('');
    $("#txtEquipGreatAndEqual").val('');

    $("#tblOpenCalls").css("display", "none");
    $("#txtOCEqual").val('');
    $("#txtOCLessAndEqual").val('');
    $("#txtOCGreatAndEqual").val('');


    $('#tblBalance input[name="$Balance"][value="rdbAny"]').prop('checked', true);
    $("#txtBalEqual").attr("disabled", "disabled");
    $("#txtBalLessAndEqual").attr("disabled", "disabled");
    $("#txtBalGreaterAndEqual").attr("disabled", "disabled");


    $('#tblLoc input[name="$loc"][value="rdbLocAny"]').prop('checked', true);
    $("#txtLocEqual").attr("disabled", "disabled");
    $("#txtLocLessAndEqual").attr("disabled", "disabled");
    $("#txtLocGreatAndEqual").attr("disabled", "disabled");

    $('#tblEquip input[name="$equip"][value="rdbEquipAny"]').prop('checked', true);
    $("#txtEquipEqual").attr("disabled", "disabled");
    $("#txtEquipLessAndEqual").attr("disabled", "disabled");
    $("#txtEquipGreatAndEqual").attr("disabled", "disabled");

    //Added by Yashasvi Jadav
    $('#tblEquipmentCounts input[name="$equipmentcounts"][value="rdbEquipmentCountsAny"]').prop('checked', true);
    $("#txtEquipmentCountsEqual").attr("disabled", "disabled");
    $("#txtEquipmentCountsLessAndEqual").attr("disabled", "disabled");
    $("#txtEquipmentCountsGreatAndEqual").attr("disabled", "disabled");

    $('#tblOpenCalls input[name="$oc"][value="rdbOCAny"]').prop('checked', true);
    $("#txtOCEqual").attr("disabled", "disabled");
    $("#txtOCLessAndEqual").attr("disabled", "disabled");
    $("#txtOCGreatAndEqual").attr("disabled", "disabled");

    $('#tblEquipmentPrice input[name="$ep"][value="rdbEquipmentPriceAny"]').prop('checked', true);
    $("#txtEquipmentPriceEqual").attr("disabled", "disabled");
    $("#txtEquipmentPriceLessAndEqual").attr("disabled", "disabled");
    $("#txtEquipmentPriceGreatAndEqual").attr("disabled", "disabled");
}

function setFilters(filterName) {
    //    if (filterName == "Name") {
    //        $("#tblName").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblName").css("display", "none");
    //    }

    //    if (filterName == "City") {
    //        $("#tblCity").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblCity").css("display", "none");
    //    }

    //    if (filterName == "State") {
    //        $("#tblState").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblState").css("display", "none");
    //    }

    //    if (filterName == "Zip") {
    //        $("#tblZip").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblZip").css("display", "none");
    //    }

    //    if (filterName == "Phone") {
    //        $("#tblPhone").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblPhone").css("display", "none");
    //    }

    //    if (filterName == "Fax") {
    //        $("#tblFax").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblFax").css("display", "none");
    //    }

    //    if (filterName == "Contact") {
    //        $("#tblContact").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblContact").css("display", "none");
    //    }

    //    if (filterName == "Address") {
    //        $("#tblAddress").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblAddress").css("display", "none");
    //    }

    //    if (filterName == "Email") {
    //        $("#tblEmail").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblEmail").css("display", "none");
    //    }

    //    if (filterName == "Country") {
    //        $("#tblCountry").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblCountry").css("display", "none");
    //    }

    //    if (filterName == "Website") {
    //        $("#tblWebsite").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblWebsite").css("display", "none");
    //    }

    //    if (filterName == "Cellular") {
    //        $("#tblCellular").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblCellular").css("display", "none");
    //    }


    //    if (filterName == "Category") {
    //        $("#tblCategory").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblCategory").css("display", "none");
    //    }

    //    if (filterName == "Type") {
    //        $("#tblType").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblType").css("display", "none");
    //    }

    //    if (filterName == "Balance") {
    //        $("#tblBalance").fadeIn(200).css("display", "block");
    //        if ($("#txtBalEqual").val() == '' && $("#txtBalLessAndEqual").val() == '' && $("#txtBalGreaterAndEqual").val() == '') {
    //            $('#tblBalance input[name="$Balance"][value="rdbAny"]').prop('checked', true);
    //            $("#txtBalEqual").attr("disabled", "disabled");
    //            $("#txtBalLessAndEqual").attr("disabled", "disabled");
    //            $("#txtBalGreaterAndEqual").attr("disabled", "disabled");
    //        }

    //    }
    //    else {
    //        $("#tblBalance").css("display", "none");
    //    }


    //    if (filterName == "Status") {
    //        $("#tblStatus").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblStatus").css("display", "none");
    //    }

    //    if (filterName == "LocationId") {
    //        $("#tblLocationId").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblLocationId").css("display", "none");
    //    }

    //    if (filterName == "LocationName") {
    //        $("#tblLocationName").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblLocationName").css("display", "none");
    //    }

    //    if (filterName == "LocationAddress") {
    //        $("#tblLocationAddress").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblLocationAddress").css("display", "none");
    //    }

    //    if (filterName == "LocationCity") {
    //        $("#tblLocationCity").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblLocationCity").css("display", "none");
    //    }

    //    if (filterName == "LocationState") {
    //        $("#tblLocationState").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblLocationState").css("display", "none");
    //    }

    //    if (filterName == "LocationZip") {
    //        $("#tblLocationZip").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblLocationZip").css("display", "none");
    //    }

    //    if (filterName == "LocationType") {
    //        $("#tblLocationType").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblLocationType").css("display", "none");
    //    }

    //    if (filterName == "EquipmentName") {
    //        $("#tblEquipmentName").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblEquipmentName").css("display", "none");
    //    }

    //    if (filterName == "EquipmentName") {
    //        $("#tblEquipmentName").fadeIn(200).css("display", "block");
    //    }
    //    else {
    //        $("#tblEquipmentName").css("display", "none");
    //    }

    debugger;
    $("#Div2 table").css("display", "none");
    if (filterName != "loc" || filterName != "equip" || filterName != "opencall") {
        filterName = filterName.replace(" ", "");
        $("#tbl" + filterName).fadeIn(200).css("display", "block");
    }

    if (filterName == "Balance") {
        $("#tblBalance").fadeIn(200).css("display", "block");
        if ($("#txtBalEqual").val() == '' && $("#txtBalLessAndEqual").val() == '' && $("#txtBalGreaterAndEqual").val() == '') {
            $('#tblBalance input[name="$Balance"][value="rdbAny"]').prop('checked', true);
            $("#txtBalEqual").attr("disabled", "disabled");
            $("#txtBalLessAndEqual").attr("disabled", "disabled");
            $("#txtBalGreaterAndEqual").attr("disabled", "disabled");
        }

    }
    //    else {
    //        $("#tblBalance").css("display", "none");
    //    }

    if (filterName == "loc") {
        $("#tblLoc").fadeIn(200).css("display", "block");
        if ($("#txtLocEqual").val() == '' && $("#txtLocLessAndEqual").val() == '' && $("#txtLocGreatAndEqual").val() == '') {
            $('#tblLoc input[name="$loc"][value="rdbLocAny"]').prop('checked', true);
            $("#txtLocEqual").attr("disabled", "disabled");
            $("#txtLocLessAndEqual").attr("disabled", "disabled");
            $("#txtLocGreatAndEqual").attr("disabled", "disabled");
        }

    }
    //    else {

    //        $("#tblLoc").css("display", "none");
    //    }

    if (filterName == "equip") {
        $("#tblEquip").fadeIn(200).css("display", "block");
        if ($("#txtEquipEqual").val() == '' && $("#txtEquipLessAndEqual").val() == '' && $("#txtEquipGreatAndEqual").val() == '') {
            $('#tblEquip input[name="$equip"][value="rdbEquipAny"]').prop('checked', true);
            $("#txtEquipEqual").attr("disabled", "disabled");
            $("#txtEquipLessAndEqual").attr("disabled", "disabled");
            $("#txtEquipGreatAndEqual").attr("disabled", "disabled");
        }

    }

    if (filterName == "EquipmentCounts") {
        $("#tblEquipmentCounts").fadeIn(200).css("display", "block");
        if ($("#txtEquipmentCountsEqual").val() == '' && $("#txtEquipmentCountsLessAndEqual").val() == '' && $("#txtEquipmentCountsGreatAndEqual").val() == '') {
            $('#tblEquipmentCounts input[name="$equipmentcounts"][value="rdbEquipmentCountsAny"]').prop('checked', true);
            $("#txtEquipmentCountsEqual").attr("disabled", "disabled");
            $("#txtEquipmentCountsLessAndEqual").attr("disabled", "disabled");
            $("#txtEquipmentCountsGreatAndEqual").attr("disabled", "disabled");
        }

    }

    //    else {

    //        $("#tblEquip").css("display", "none");
    //    }

    if (filterName == "opencall") {
        $("#tblOpenCalls").fadeIn(200).css("display", "block");
        if ($("#txtOCEqual").val() == '' && $("#txtOCLessAndEqual").val() == '' && $("#txtOCGreatAndEqual").val() == '') {
            $('#tblOpenCalls input[name="$oc"][value="rdbOCAny"]').prop('checked', true);
            $("#txtOCEqual").attr("disabled", "disabled");
            $("#txtOCLessAndEqual").attr("disabled", "disabled");
            $("#txtOCGreatAndEqual").attr("disabled", "disabled");
        }

    }
    //    else {

    //        $("#tblOpenCalls").css("display", "none");
    //    }

    if (filterName == "EquipmentPrice") {
        $("#tblEquipmentPrice").fadeIn(200).css("display", "block");
        if ($("#txtEquipmentPriceEqual").val() == '' && $("#txtEquipmentPriceLessAndEqual").val() == '' && $("#txtEquipmentPriceGreatAndEqual").val() == '') {
            $('#tblEquipmentPrice input[name="$ep"][value="rdbEquipmentPriceAny"]').prop('checked', true);
            $("#txtEquipmentPriceEqual").attr("disabled", "disabled");
            $("#txtEquipmentPriceLessAndEqual").attr("disabled", "disabled");
            $("#txtEquipmentPriceGreatAndEqual").attr("disabled", "disabled");
        }

    }
}

function setFiltersValue(filterName, filterValue) {
    if (filterName == "Name") {
        //$("#txtName").val(filterValue);
        //                filterValue == "" ? ($("#drpName").val('All')) : ($("#drpName option:contains('" + filterValue + "')").prop('selected', true));
        // filterValue == "" ? ($("#drpName").val('All')) : ($("#drpName").val(filterValue));
        $('#drpName input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#drpName input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#drpName input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "Equipment") {
        //$("#txtName").val(filterValue);
        //                filterValue == "" ? ($("#drpName").val('All')) : ($("#drpName option:contains('" + filterValue + "')").prop('selected', true));
        // filterValue == "" ? ($("#drpName").val('All')) : ($("#drpName").val(filterValue));
        $('#drpEquipmentState input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#drpEquipmentState input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#drpEquipmentState input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "City") {
        //                $("#txtCity").val(filterValue);
        $('#drpCity input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedCity = filterValue.split("|");
            for (i = 0; i <= splitedCity.length - 1; i++) {
                $('#drpCity input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedCity[i] = splitedCity[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedCity[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#drpCity input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "State") {
        //                filterValue == "" ? ($("#ddlState").val('All')) : ($("#ddlState option:contains('" + filterValue + "')").prop('selected', true));
        //  filterValue == "" ? ($("#ddlState").val('All')) : ($("#ddlState").val(filterValue));
        $('#ddlState input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#ddlState input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    actualText = splitedNames[i].replace("'", "").replace("'", "");
                    // alert(actualText);
                    // alert(actualText.trim().length);
                    if (actualText.trim().length <= 2) {

                        $("#ddlStateReference option[value=" + actualText.trim() + "]").attr("selected", "selected");
                    }
                    else {
                        $("#ddlStateReference option:contains('" + actualText.trim() + "')").attr("selected", "selected")
                    }

                    var getText = $("#ddlStateReference option:selected").text();
                    // alert(getText);
                    if (lblValue.trim() == getText.trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#ddlState input[type="checkbox"]').attr('checked', false);
        }

    }

    if (filterName == "LocationState") {
        //                filterValue == "" ? ($("#ddlState").val('All')) : ($("#ddlState option:contains('" + filterValue + "')").prop('selected', true));
        //  filterValue == "" ? ($("#ddlState").val('All')) : ($("#ddlState").val(filterValue));
        $('#drpLocationState input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedLocStates = filterValue.split("|");
            for (i = 0; i <= splitedLocStates.length - 1; i++) {
                $('#drpLocationState input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    actualText = splitedLocStates[i].replace("'", "").replace("'", "");
                    // alert(actualText);
                    // alert(actualText.trim().length);
                    if (actualText.trim().length <= 2) {

                        $("#ddlStateReference option[value=" + actualText.trim() + "]").attr("selected", "selected");
                    }
                    else {
                        $("#ddlStateReference option:contains('" + actualText.trim() + "')").attr("selected", "selected")
                    }

                    var getText = $("#ddlStateReference option:selected").text();
                    // alert(getText);
                    if (lblValue.trim() == getText.trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#drpLocationState input[type="checkbox"]').attr('checked', false);
        }

    }

    if (filterName == "Zip") {
        $("#txtZip").val(filterValue);
    }

    if (filterName == "InstalledOn") {
        $("#InstalledOn").val(filterValue);
    }

    if (filterName == "Phone") {
        $("#txtPhone").val(filterValue);
    }

    if (filterName == "Fax") {
        $("#txtFax").val(filterValue);
    }

    if (filterName == "Contact") {
        $("#txtContact").val(filterValue);
    }

    if (filterName == "Address") {
        //                $("#txtAddress").val(filterValue);
        $('#drpAddress input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedAddress = filterValue.split("|");
            for (i = 0; i <= splitedAddress.length - 1; i++) {
                $('#drpAddress input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedAddress[i] = splitedAddress[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedAddress[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#drpAddress input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "Email") {
        $("#txtEmail").val(filterValue);
    }

    if (filterName == "Country") {
        $("#txtCountry").val(filterValue);
    }

    if (filterName == "Website") {
        $("#txtWebsite").val(filterValue);
    }

    if (filterName == "Cellular") {
        $("#txtCellular").val(filterValue);
    }

    if (filterName == "Category") {
        filterValue == "" ? ($("#drpCategory").val('All')) : ($("#drpCategory option:selected").val(filterValue));
    }

    if (filterName == "Type") {
        // filterValue == "" ? ($("#drpType").val('All')) : ($("#drpType option:contains('" + filterValue + "')").prop('selected', true));
        $('#drpType input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedTypes = filterValue.split("|");
            for (i = 0; i <= splitedTypes.length - 1; i++) {
                $('#drpType input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedTypes[i] = splitedTypes[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedTypes[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#drpType input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "Balance") {
        if (filterValue == "") {
            $('#tblBalance input[name="$Balance"][value="rdbAny"]').prop('checked', true);
            $("#txtBalEqual").val('');
            $("#txtBalEqual").attr("disabled", true);
            $("#txtBalGreaterAndEqual").val('');
            $("#txtBalGreaterAndEqual").attr("disabled", true);
            $("#txtBalLessAndEqual").val('');
            $("#txtBalLessAndEqual").attr("disabled", true);
        }
        else if ((filterValue.indexOf(">=") != -1 || filterValue.indexOf("&gt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#txtBalGreaterAndEqual").attr("disabled", false);
            $('#tblBalance input[name="$Balance"][value="rdbGreaterAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#txtBalGreaterAndEqual").val(filterValue.trim());
            $("#txtBalEqual").val('');
            $("#txtBalEqual").attr("disabled", 'disabled');
            $("#txtBalLessAndEqual").val('');
            $("#txtBalLessAndEqual").attr("disabled", 'disabled');
        }
        else if ((filterValue.indexOf("<=") != -1 || filterValue.indexOf("&lt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#txtBalLessAndEqual").attr("disabled", false);
            $('#tblBalance input[name="$Balance"][value="rdbLessAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#txtBalLessAndEqual").val(filterValue.trim());
            $("#txtBalEqual").val('');
            $("#txtBalEqual").attr("disabled", true);
            $("#txtBalGreaterAndEqual").val('');
            $("#txtBalGreaterAndEqual").attr("disabled", true);
        }
        else if (filterValue.indexOf('and') != -1) {
            var splitedBalance = '';
            var greaterValue = '';
            var lessValue = '';
            splitedBalance = filterValue.split('and');
            greaterValue = splitedBalance[0].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            lessValue = splitedBalance[1].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#txtBalGreaterAndEqual").val(greaterValue.trim());
            $("#txtBalLessAndEqual").val(lessValue.trim());
            $("#txtBalEqual").val('');
            $("#txtBalEqual").attr("disabled", true);
            $("#txtBalGreaterAndEqual").attr("disabled", false);
            $("#txtBalLessAndEqual").attr("disabled", false);
            $('#tblBalance input[name="$Balance"][value="rdbLessAndEqual"]').prop('checked', true);
        }
        else {
            $('#tblBalance input[name="$Balance"][value="rdbEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#txtBalEqual").val(filterValue.trim());
            $("#txtBalGreaterAndEqual").val('');
            $("#txtBalGreaterAndEqual").attr("disabled", true);
            $("#txtBalLessAndEqual").val('');
            $("#txtBalLessAndEqual").attr("disabled", true);

        }

        //                filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
        //                $("#txtBalance").val(filterValue.trim());
    }


    if (filterName == "loc") {
        if (filterValue == "") {
            $('#tblLoc input[name="$loc"][value="rdbLocAny"]').prop('checked', true);
            $("#txtLocEqual").val('');
            $("#txtLocEqual").attr("disabled", true);
            $("#txtLocLessAndEqual").val('');
            $("#txtLocLessAndEqual").attr("disabled", true);
            $("#txtLocGreatAndEqual").val('');
            $("#txtLocGreatAndEqual").attr("disabled", true);
        }
            //else if ((filterValue.contains(">=") || filterValue.contains("&gt;")) && !filterValue.contains('and')) {
        else if ((filterValue.indexOf(">=") != -1 || filterValue.indexOf("&gt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#txtLocGreatAndEqual").attr("disabled", false);
            $('#tblLoc input[name="$loc"][value="rdbLocGreaterAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#txtLocGreatAndEqual").val(filterValue.trim());
            $("#txtLocEqual").val('');
            $("#txtLocEqual").attr("disabled", 'disabled');
            $("#txtLocLessAndEqual").val('');
            $("#txtLocLessAndEqual").attr("disabled", 'disabled');
        }
        else if ((filterValue.indexOf("<=") != -1 || filterValue.indexOf("&lt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#txtLocLessAndEqual").attr("disabled", false);
            $('#tblLoc input[name="$loc"][value="rdbLocLessAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#txtLocLessAndEqual").val(filterValue.trim());
            $("#txtLocEqual").val('');
            $("#txtLocEqual").attr("disabled", true);
            $("#txtLocGreatAndEqual").val('');
            $("#txtLocGreatAndEqual").attr("disabled", true);
        }
        else if (filterValue.indexOf('and') != -1) {
            var splitedLoc = '';
            var greaterValue = '';
            var lessValue = '';
            splitedLoc = filterValue.split('and');
            greaterValue = splitedLoc[0].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            lessValue = splitedLoc[1].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#txtLocGreatAndEqual").val(greaterValue.trim());
            $("#txtLocLessAndEqual").val(lessValue.trim());
            $("#txtLocEqual").val('');
            $("#txtLocEqual").attr("disabled", true);
            $("#txtLocGreatAndEqual").attr("disabled", false);
            $("#txtLocLessAndEqual").attr("disabled", false);
            $('#tblLoc input[name="$loc"][value="rdbLocLessAndEqual"]').prop('checked', true);
        }
        else {
            $('#tblLoc input[name="$loc"][value="rdbLocEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#txtLocEqual").val(filterValue.trim());
            $("#txtLocGreatAndEqual").val('');
            $("#txtLocGreatAndEqual").attr("disabled", true);
            $("#txtLocLessAndEqual").val('');
            $("#txtLocLessAndEqual").attr("disabled", true);

        }
    }

    if (filterName == "equip") {
        if (filterValue == "") {
            $('#tblEquip input[name="$equip"][value="rdbEquipAny"]').prop('checked', true);
            $("#txtEquipEqual").val('');
            $("#txtEquipEqual").attr("disabled", true);
            $("#txtEquipLessAndEqual").val('');
            $("#txtEquipLessAndEqual").attr("disabled", true);
            $("#txtEquipGreatAndEqual").val('');
            $("#txtEquipGreatAndEqual").attr("disabled", true);
        }
        else if ((filterValue.indexOf(">=") != -1 || filterValue.indexOf("&gt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#txtEquipGreatAndEqual").attr("disabled", false);
            $('#tblEquip input[name="$equip"][value="rdbEquipGreaterAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#txtEquipGreatAndEqual").val(filterValue.trim());
            $("#txtEquipEqual").val('');
            $("#txtEquipEqual").attr("disabled", 'disabled');
            $("#txtEquipLessAndEqual").val('');
            $("#txtEquipLessAndEqual").attr("disabled", 'disabled');
        }
        else if ((filterValue.indexOf("<=") != -1 || filterValue.indexOf("&lt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#txtEquipLessAndEqual").attr("disabled", false);
            $('#tblEquip input[name="$equip"][value="rdbEquipLessAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#txtEquipLessAndEqual").val(filterValue.trim());
            $("#txtEquipEqual").val('');
            $("#txtEquipEqual").attr("disabled", true);
            $("#txtEquipGreatAndEqual").val('');
            $("#txtEquipGreatAndEqual").attr("disabled", true);
        }
        else if (filterValue.indexOf('and') != -1) {
            var splitedEquip = '';
            var greaterValue = '';
            var lessValue = '';
            splitedEquip = filterValue.split('and');
            greaterValue = splitedEquip[0].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            lessValue = splitedEquip[1].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#txtEquipGreatAndEqual").val(greaterValue.trim());
            $("#txtEquipLessAndEqual").val(lessValue.trim());
            $("#txtEquipEqual").val('');
            $("#txtEquipEqual").attr("disabled", true);
            $("#txtEquipGreatAndEqual").attr("disabled", false);
            $("#txtEquipLessAndEqual").attr("disabled", false);
            $('#tblEquip input[name="$equip"][value="rdbEquipLessAndEqual"]').prop('checked', true);
        }
        else {
            $('#tblEquip input[name="$equip"][value="rdbEquipEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#txtEquipEqual").val(filterValue.trim());
            $("#txtEquipGreatAndEqual").val('');
            $("#txtEquipGreatAndEqual").attr("disabled", true);
            $("#txtEquipLessAndEqual").val('');
            $("#txtEquipLessAndEqual").attr("disabled", true);

        }
    }

    if (filterName == "EquipmentCounts") {
        if (filterValue == "") {
            $('#tblEquipmentCounts input[name="$equipmentcounts"][value="rdbEquipmentCountsAny"]').prop('checked', true);
            $("#txtEquipmentCountsEqual").val('');
            $("#txtEquipmentCountsEqual").attr("disabled", true);
            $("#txtEquipmentCountsLessAndEqual").val('');
            $("#txtEquipmentCountsLessAndEqual").attr("disabled", true);
            $("#txtEquipmentCountsGreatAndEqual").val('');
            $("#txtEquipmentCountsGreatAndEqual").attr("disabled", true);
        }
        else if ((filterValue.indexOf(">=") != -1 || filterValue.indexOf("&gt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#txtEquipmentCountsGreatAndEqual").attr("disabled", false);
            $('#tblEquipmentCounts input[name="$equipmentcounts"][value="rdbEquipmentCountsGreaterAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#txtEquipmentCountsGreatAndEqual").val(filterValue.trim());
            $("#txtEquipmentCountsEqual").val('');
            $("#txtEquipmentCountsEqual").attr("disabled", 'disabled');
            $("#txtEquipmentCountsLessAndEqual").val('');
            $("#txtEquipmentCountsLessAndEqual").attr("disabled", 'disabled');
        }
        else if ((filterValue.indexOf("<=") != -1 || filterValue.indexOf("&lt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#txtEquipmentCountsLessAndEqual").attr("disabled", false);
            $('#tblEquipmentCounts input[name="$equipmentcounts"][value="rdbEquipmentCountsLessAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#txtEquipmentCountsLessAndEqual").val(filterValue.trim());
            $("#txtEquipmentCountsEqual").val('');
            $("#txtEquipmentCountsEqual").attr("disabled", true);
            $("#txtEquipmentCountsGreatAndEqual").val('');
            $("#txtEquipmentCountsGreatAndEqual").attr("disabled", true);
        }
        else if (filterValue.indexOf('and') != -1) {
            var splitedEquip = '';
            var greaterValue = '';
            var lessValue = '';
            splitedEquip = filterValue.split('and');
            greaterValue = splitedEquip[0].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            lessValue = splitedEquip[1].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#txtEquipmentCountsGreatAndEqual").val(greaterValue.trim());
            $("#txtEquipmentCountsLessAndEqual").val(lessValue.trim());
            $("#txtEquipmentCountsEqual").val('');
            $("#txtEquipmentCountsEqual").attr("disabled", true);
            $("#txtEquipmentCountsGreatAndEqual").attr("disabled", false);
            $("#txtEquipmentCountsLessAndEqual").attr("disabled", false);
            $('#tblEquipmentCounts input[name="$equipmentcounts"][value="rdbEquipmentCountsLessAndEqual"]').prop('checked', true);
        }
        else {
            $('#tblEquipmentCounts input[name="$equipmentcounts"][value="rdbEquipmentCountsEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#txtEquipmentCountsEqual").val(filterValue.trim());
            $("#txtEquipmentCountsGreatAndEqual").val('');
            $("#txtEquipmentCountsGreatAndEqual").attr("disabled", true);
            $("#txtEquipmentCountsLessAndEqual").val('');
            $("#txtEquipmentCountsLessAndEqual").attr("disabled", true);

        }
    }

    if (filterName == "opencall") {
        if (filterValue == "") {
            $('#tblOpenCalls input[name="$oc"][value="rdbOCAny"]').prop('checked', true);
            $("#txtOCEqual").val('');
            $("#txtOCEqual").attr("disabled", true);
            $("#txtOCLessAndEqual").val('');
            $("#txtOCLessAndEqual").attr("disabled", true);
            $("#txtOCGreatAndEqual").val('');
            $("#txtOCGreatAndEqual").attr("disabled", true);
        }
        else if ((filterValue.indexOf(">=") != -1 || filterValue.indexOf("&gt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#txtOCGreatAndEqual").attr("disabled", false);
            $('#tblOpenCalls input[name="$oc"][value="rdbOCGreaterAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#txtOCGreatAndEqual").val(filterValue.trim());
            $("#txtOCEqual").val('');
            $("#txtOCEqual").attr("disabled", 'disabled');
            $("#txtOCLessAndEqual").val('');
            $("#txtOCLessAndEqual").attr("disabled", 'disabled');
        }
        else if ((filterValue.indexOf("<=") != -1 || filterValue.indexOf("&lt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#txtOCLessAndEqual").attr("disabled", false);
            $('#tblOpenCalls input[name="$oc"][value="rdbOCLessAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#txtOCLessAndEqual").val(filterValue.trim());
            $("#txtOCEqual").val('');
            $("#txtOCEqual").attr("disabled", true);
            $("#txtOCGreatAndEqual").val('');
            $("#txtOCGreatAndEqual").attr("disabled", true);
        }
        else if (filterValue.indexOf('and') != -1) {
            var splitedOC = '';
            var greaterValue = '';
            var lessValue = '';
            splitedOC = filterValue.split('and');
            greaterValue = splitedOC[0].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            lessValue = splitedOC[1].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#txtOCGreatAndEqual").val(greaterValue.trim());
            $("#txtOCLessAndEqual").val(lessValue.trim());
            $("#txtOCEqual").val('');
            $("#txtOCEqual").attr("disabled", true);
            $("#txtOCGreatAndEqual").attr("disabled", false);
            $("#txtOCLessAndEqual").attr("disabled", false);
            $('#tblOpenCalls input[name="$oc"][value="rdbOCLessAndEqual"]').prop('checked', true);
        }
        else {
            $('#tblOpenCalls input[name="$oc"][value="rdbOCEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#txtOCEqual").val(filterValue.trim());
            $("#txtOCGreatAndEqual").val('');
            $("#txtOCGreatAndEqual").attr("disabled", true);
            $("#txtOCLessAndEqual").val('');
            $("#txtOCLessAndEqual").attr("disabled", true);

        }

    }

    if (filterName == "EquipmentPrice") {
        if (filterValue == "") {
            $('#tblEquipmentPrice input[name="$ep"][value="rdbEquipmentPriceAny"]').prop('checked', true);
            $("#txtEquipmentPriceEqual").val('');
            $("#txtEquipmentPriceEqual").attr("disabled", true);
            $("#txtEquipmentPriceLessAndEqual").val('');
            $("#txtEquipmentPriceLessAndEqual").attr("disabled", true);
            $("#txtEquipmentPriceGreatAndEqual").val('');
            $("#txtEquipmentPriceGreatAndEqual").attr("disabled", true);
        }
        else if ((filterValue.indexOf(">=") != -1 || filterValue.indexOf("&gt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#txtEquipmentPriceGreatAndEqual").attr("disabled", false);
            $('#tblEquipmentPrice input[name="$ep"][value="rdbEquipmentPriceGreaterAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#txtEquipmentPriceGreatAndEqual").val(filterValue.trim());
            $("#txtEquipmentPriceEqual").val('');
            $("#txtEquipmentPriceEqual").attr("disabled", 'disabled');
            $("#txtEquipmentPriceLessAndEqual").val('');
            $("#txtEquipmentPriceLessAndEqual").attr("disabled", 'disabled');
        }
        else if ((filterValue.indexOf("<=") != -1 || filterValue.indexOf("&lt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#txtEquipmentPriceLessAndEqual").attr("disabled", false);
            $('#tblEquipmentPrice input[name="$ep"][value="rdbEquipmentPriceLessAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#txtEquipmentPriceLessAndEqual").val(filterValue.trim());
            $("#txtEquipmentPriceEqual").val('');
            $("#txtEquipmentPriceEqual").attr("disabled", true);
            $("#txtEquipmentPriceGreatAndEqual").val('');
            $("#txtEquipmentPriceGreatAndEqual").attr("disabled", true);
        }
        else if (filterValue.indexOf('and') != -1) {
            var splitedOC = '';
            var greaterValue = '';
            var lessValue = '';
            splitedOC = filterValue.split('and');
            greaterValue = splitedOC[0].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            lessValue = splitedOC[1].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#txtEquipmentPriceGreatAndEqual").val(greaterValue.trim());
            $("#txtEquipmentPriceLessAndEqual").val(lessValue.trim());
            $("#txtEquipmentPriceEqual").val('');
            $("#txtEquipmentPriceEqual").attr("disabled", true);
            $("#txtEquipmentPriceGreatAndEqual").attr("disabled", false);
            $("#txtEquipmentPriceLessAndEqual").attr("disabled", false);
            $('#tblEquipmentPrice input[name="$ep"][value="rdbEquipmentPriceLessAndEqual"]').prop('checked', true);
        }
        else {
            $('#tblEquipmentPrice input[name="$ep"][value="rdbEquipmentPriceEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#txtEquipmentPriceEqual").val(filterValue.trim());
            $("#txtEquipmentPriceGreatAndEqual").val('');
            $("#txtEquipmentPriceGreatAndEqual").attr("disabled", true);
            $("#txtEquipmentPriceLessAndEqual").val('');
            $("#txtEquipmentPriceLessAndEqual").attr("disabled", true);

        }

    }


    if (filterName == "Status") {
        filterValue == "" ? ($("#drpStatus").val('All')) : ($("#drpStatus  option:contains('" + filterValue + "')").prop('selected', true));
    }

    if (filterName == "Location Id") {

        $('#drpLocationId input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#drpLocationId input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#drpLocationId input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "Loc Type") {

        $('#drpLocationType input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#drpLocationType input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#drpLocationType input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "Service Type") {

        $('#drpServiceType input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#drpServiceType input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#drpServiceType input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "Preferred Worker") {

        $('#drpPreferredWorker input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#drpPreferredWorker input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#drpPreferredWorker input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "Ticket Start") {

        $('#drpTicketStart input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#drpTicketStart input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#drpTicketStart input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "Ticket Time") {

        $('#drpTicketTime input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#drpTicketTime input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#drpTicketTime input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "Ticket Freq") {

        $('#drpTicketFreq input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#drpTicketFreq input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#drpTicketFreq input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "Bill Start") {

        $('#drpBillStart input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#drpBillStart input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#drpBillStart input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "Bill Amount") {

        $('#drpBillAmount input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#drpBillAmount input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#drpBillAmount input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "Bill Freqency") {

        $('#drpBillFreqency input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#drpBillFreqency input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#drpBillFreqency input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "Expiration Date") {

        $('#drpExpirationDate input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#drpExpirationDate input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#drpExpirationDate input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "Phone Monitoring") {

        $('#drpPhoneMonitoring input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#drpPhoneMonitoring input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#drpPhoneMonitoring input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "Contract Type") {

        $('#drpContractType input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#drpContractType input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#drpContractType input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "Occupancy Discount") {

        $('#drpOccupancyDiscount input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#drpOccupancyDiscount input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#drpOccupancyDiscount input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "Term Of Contract") {

        $('#drpTermOfContract input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#drpTermOfContract input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#drpTermOfContract input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "Price Adjustment Cap") {

        $('#drpPriceAdjustmentCap input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#drpPriceAdjustmentCap input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#drpPriceAdjustmentCap input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "Fire Service Testing Included") {

        $('#drpFireServiceTestingIncluded input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#drpFireServiceTestingIncluded input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#drpFireServiceTestingIncluded input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "Special Rates") {

        $('#drpSpecialRates input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#drpSpecialRates input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#drpSpecialRates input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "Contract Expiration") {

        $('#drpContractExpiration input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#drpContractExpiration input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#drpContractExpiration input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "Prorated Items") {

        $('#drpProratedItems input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#drpProratedItems input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#drpProratedItems input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "Annual Test Included") {

        $('#drpAnnualTestIncluded input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#drpAnnualTestIncluded input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#drpAnnualTestIncluded input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "Five Year State Test Included") {

        $('#drpFiveYearStateTestIncluded input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#drpFiveYearStateTestIncluded input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#drpFiveYearStateTestIncluded input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "Fire Service Tested Included") {

        $('#drpFireServiceTestedIncluded input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#drpFireServiceTestedIncluded input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#drpFireServiceTestedIncluded input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "Cancellation Notification Days") {

        $('#drpCancellationNotificationDays input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#drpCancellationNotificationDays input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#drpCancellationNotificationDays input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "Price Adjustment Notification Days") {

        $('#drpPriceAdjustmentNotificationDays input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#drpPriceAdjustmentNotificationDays input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#drpPriceAdjustmentNotificationDays input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "After Hours Calls Included") {

        $('#drpAfterHoursCallsIncluded input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#drpAfterHoursCallsIncluded input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#drpAfterHoursCallsIncluded input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "OG Service Calls Included") {

        $('#drpOGServiceCallsIncluded input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#drpOGServiceCallsIncluded input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#drpOGServiceCallsIncluded input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "Contract Hours") {

        $('#drpContractHours input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#drpContractHours input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#drpContractHours input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "Contract Format") {

        $('#drpContractFormat input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#drpContractFormat input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#drpContractFormat input[type="checkbox"]').attr('checked', false);
        }
    }
    //Rahil
    //  if (filterName == "LocationId") {
    // filterValue == "" ? ($("#drpType").val('All')) : ($("#drpType option:contains('" + filterValue + "')").prop('selected', true));

    //if (filterName == "TicketTime" || filterName == "ServiceType" || filterName == "TicketStart" || filterName == "Hours" || filterName == "BillFreqency" || filterName == "PreferredWorker" || filterName == "BillStart" || filterName == "TicketFreq" || filterName == "BillAmount" || filterName == "Description" || filterName == "LocationId" || filterName == "LocationName" || filterName == "LocationAddress" || filterName == "LocationCity" || filterName == "LocationZip" || filterName == "LocationType" || filterName == "EquipmentName" || filterName == "OGServiceCallsIncluded" || filterName == "EquipmentType" || filterName == "Customer" || filterName == "ContractHours" || filterName == "ContractFormat" || filterName == "AfterHoursCallsIncluded" || filterName == "PriceAdjustmentNotificationDays" || filterName == "CancellationNotificationDays" || filterName == "FireServiceTestedIncluded" || filterName == "FiveYearStateTestIncluded" || filterName == "AnnualTestIncluded" || filterName == "ProratedItems" || filterName == "ContractExpiration" || filterName == "SpecialRates" || filterName == "FireServiceTestingIncluded" || filterName == "PriceAdjustmentCap" || filterName == "TermOfContract" || filterName == "Exclusions" || filterName == "OccupancyDiscount" || filterName == "ContractType" || filterName == "PhoneMonitoring" || filterName == "ExpirationDate" || filterName == "Expiration" || filterName == "Equipment") {
    if (filterName == "Hours" || filterName == "Description" || filterName == "Customer" ||  filterName == "Exclusions" || filterName == "Expiration" || filterName == "Equipment") {

        $('#drp' + filterName + ' input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedValues = filterValue.split("|");
            for (i = 0; i <= splitedValues.length - 1; i++) {
                $('#drp' + filterName + ' input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedValues[i] = splitedValues[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedValues[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#drp' + filterName + ' input[type="checkbox"]').attr('checked', false);
        }
    }

}


$(function () {
    var appendthis = ("<div class='modal-overlay js-modal-close'></div>");

    $('a[data-modal-id]').click(function (e) {
        e.preventDefault();
        $("body").append(appendthis);
        //$(".modal-overlay").fadeTo(500, 0.7);
        //$(".js-modalbox").fadeIn(500);
        var modalBox = $(this).attr('data-modal-id');
        $('#' + modalBox).fadeIn($(this).data());
    });


    $(".js-modal-close, .modal-overlay").click(function () {
        $(".modal-box, .modal-overlay").fadeOut(500, function () {
            $(".modal-overlay").remove();
        });
    });

    $(window).resize(function () {
        $(".modal-box").css({
            top: ($(window).height() - $(".modal-box").outerHeight()) / 2,
            left: ($(window).width() - $(".modal-box").outerWidth()) / 2
        });
    });

    $(window).resize();

});


jQuery(document).ready(function () {

    $('#colorNav #dynamicUI li').remove();
    $('#colorNav #dynamicUI').append('<li><a onClick="ExportToPDF();"><img src="images/PDFIcon.png" width="18px" style="float:left;padding:5px 10px;" /><span style="float:left;padding-top:5px;">Export to PDF</span><div style="clear:both;"></div></a></li>')
    $('#colorNav #dynamicUI').append('<li><a onClick="ExportToExcel();"><img src="images/ExcelIcon.jpg" width="18px" style="float:left;padding:5px 10px;" /><span style="float:left;padding-top:5px;">Export to Excel</span><div style="clear:both;"></div></a></li>')

    $('#dvBtnEmail #dvEmailOptions li').remove();

    $('#dvBtnEmail #dvEmailOptions').append('<li><a id="btnSendPDFReport" href="#" onClick="SendPDFReport(this.id);"><img src="images/EmailPDFIcon.png" width="18px" style="float:left;padding:5px 10px;" /><span style="float:left;padding-top:5px;">Send report as PDF</span><div style="clear:both;"></div></a></li>')
    $('#dvBtnEmail #dvEmailOptions').append('<li><a id="btnSendExcelReport" href="#" onClick="SendPDFReport(this.id);"><img src="images/EmailExcelIcon.png" width="18px" style="float:left;padding:5px 10px;" /><span style="float:left;padding-top:5px;">Send report as Excel</span><div style="clear:both;"></div></a></li>')

    if ($("#hdnMainHeader").val() == 'True') {
        $("#dvMainHeader").show();
    }
    else {
        $("#dvMainHeader").hide();
    }
    var getColumnWidth = $("#hdnColumnWidth").val();
    var splittedColWidth = '';
    if (getColumnWidth != '') {
        splittedColWidth = $("#hdnColumnWidth").val().split(",");

        $('#tblResize tr th').each(function (index, element) {
            var colWidth = splittedColWidth[index];
            $(this).css('width', colWidth);
        });
    }

    var columnList = $("#hdnColumnList").val();
    $('#lstColumnSort option').remove();
    var splitedColList = columnList.split(",");
    $.each(splitedColList, function (key, value) {

        $('#lstColumnSort')
                                            .append($("<option></option>")
                                 .attr("value", value)
                                 .text(value));

    });

    //$("#lstFilter option[value='loc'],option[value='equip'], option[value='opencall']").remove();

    if ($("#drpReports").val() == 0) {
        $("#btnCustomizeReport").hide();
        $("#btnDeleteReport").hide();
    }
    else {
        $("#btnCustomizeReport").show();
        $("#btnDeleteReport").show();
    }


    $('[name="$CrystalReportViewer1$ctl02$ctl11"]').hide();
    $('[name="$CrystalReportViewer1$ctl02$ctl03"]').hide();

    jQuery('.tabs .tab-links a').on('click', function (e) {
        var currentAttrValue = jQuery(this).attr('href');

        // Show/Hide Tabs
        jQuery('.tabs ' + currentAttrValue).show().siblings().hide();

        // Change/remove current tab to active
        jQuery(this).parent('li').addClass('active').siblings().removeClass('active');

        e.preventDefault();
    });

    $("#btnPrint").click(function () {
        $("#dvGridReport").print();
        return (false);
    });

    $("#btnApply, #btnPreviewApply").click(function () {
        var isChecked = null;
        var chkArray = [];

        $('#chkColumnList input[type=checkbox]:checked').each(function () {
            isChecked = true;
            //chkArray.push($(this).parent().find('label').html());
        });
        if (isChecked) {

            $("#popup").hide();
            $("#dvSaveReport").show();
            var drpSortByVal = $('#drpSortBy option:selected').val();
            $("#hdnDrpSortBy").val(drpSortByVal);

        }
        else {
            $("#dvSaveReport").hide();
            alert("Please select at least one field!");
            return;
        }

        var lstSortColumn = '';
        $('#lstColumnSort option').each(function (index, element) {
            //alert($(element).val());
            lstSortColumn += $(element).val() + "^";

        });

        $("#hdnLstColumns").val(lstSortColumn);

        var lstColumnWidth = '';
        $('#tblResize tr th').each(function () {
            lstColumnWidth += $(this).css('width') + "^";
        });

        $("#hdnColumnWidth").val(lstColumnWidth);

        var filterColumns = '';
        var filterValues = '';
        var checkIndex = '';
        $('#tblFilterChoices tr').each(function () {
            filterColumns += $(this).find("td:first").html() + "^";
            checkIndex = $(this).find("td:first").html();
            if (checkIndex == "EquipmentState") {
                filterValues += $(this).find("td:last").html() + "^";
            }
            else {
                if (checkIndex.indexOf("State") == -1) {
                    filterValues += $(this).find("td:last").html() + "^";
                }
                else {
                    var getStates = $(this).find("td:last").html() + "^";
                    var splitedStates = getStates.split("|");
                    for (i = 0; i < splitedStates.length; i++) {
                        actualText = $.trim(splitedStates[i].replace("'", "").replace("'", "").replace("^", ""));
                        $("#ddlStateReference option:contains('" + actualText + "')").attr("selected", "selected")
                        var getValue = $("#ddlStateReference option:selected").val();
                        filterValues += "'" + getValue + "'" + "|";
                    }
                    filterValues = filterValues.substring(0, filterValues.length - 1) + "^";
                }
            }
        });

        $("#hdnFilterColumns").val(filterColumns);
        $("#hdnFilterValues").val(filterValues);

    });

    //Added By Yashasvi Jadav
    $("#btnPreview").click(function () {
        $("#myModal").modal('show');

        $('#lblPreviewCompEmail').text("");
        $('#lblPreviewCompanyName').text("");
        $('#lblPreviewCompAddress').text("");
        $('#imgPreview').attr('src', "");
        $('#lblPreviewTime').text("");
        $('#lblPreviewDate').text("");

        var isChecked = null;
        var chkArray = [];
        var drpSortByVal = null;

        $('#chkColumnList input[type=checkbox]:checked').each(function () {
            isChecked = true;
        });
        if (isChecked) {

            $("#popup").hide();
            //$("#dvSaveReport").show();
            drpSortByVal = $('#drpSortBy option:selected').val();
            //$("#hdnDrpSortBy").val(drpSortByVal);

        }
        else {
            $("#dvSaveReport").hide();
            alert("Please select at least one field!");
            return;
        }

        var lstSortColumn = '';
        $('#lstColumnSort option').each(function (index, element) {
            lstSortColumn += $(element).val() + "^";

        });

        //$("#hdnLstColumns").val(lstSortColumn);

        var lstColumnWidth = '';
        $('#tblResize tr th').each(function () {
            lstColumnWidth += $(this).css('width') + "^";
        });

        //$("#hdnColumnWidth").val(lstColumnWidth);

        var filterColumns = '';
        var filterValues = '';
        var checkIndex = '';
        $('#tblFilterChoices tr').each(function () {
            filterColumns += $(this).find("td:first").html() + "^";
            checkIndex = $(this).find("td:first").html();
            if (checkIndex == "EquipmentState") {
                filterValues += $(this).find("td:last").html() + "^";
            }
            else {
                if (checkIndex.indexOf("State") == -1) {
                    filterValues += $(this).find("td:last").html() + "^";
                }
                else {
                    var getStates = $(this).find("td:last").html() + "^";
                    var splitedStates = getStates.split("|");
                    for (i = 0; i < splitedStates.length; i++) {
                        actualText = $.trim(splitedStates[i].replace("'", "").replace("'", "").replace("^", ""));
                        $("#ddlStateReference option:contains('" + actualText + "')").attr("selected", "selected")
                        var getValue = $("#ddlStateReference option:selected").val();
                        filterValues += "'" + getValue + "'" + "|";
                    }
                    filterValues = filterValues.substring(0, filterValues.length - 1) + "^";
                }
            }
        });

        //$("#hdnFilterColumns").val(filterColumns);
        //$("#hdnFilterValues").val(filterValues);

        var reportId = $("#drpReports").find('option:selected').val();
        $('#loading').show();
        $("#tblPreviewReport").empty();
        $.ajax({
            type: "POST",
            url: "RecContractsModule.aspx/GetRecurringPreviewDetails",
            data: JSON.stringify({ reportId: reportId, FilterColumn: filterColumns, FilterValues: filterValues, ColumnWidth: lstColumnWidth, SortColumn: lstSortColumn, DataSortBy: drpSortByVal }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response != null || response != "") {
                    var _data = JSON.parse(response.d);
                    $('#lblPreviewCompEmail').text(_data["Email"]);
                    $('#lblPreviewCompanyName').text(_data["Name"]);
                    $('#lblPreviewCompAddress').text(_data["Address"]);
                    $('#imgPreview').attr({ 'src': _data["Image"], 'width': '150px', 'height': '150px' });
                    $('#lblPreviewTime').text(_data["Time"]);
                    $('#lblPreviewDate').text(_data["Date"]);
                    $("#tblPreviewReport").append(_data["PreviewData"]);
                }
                $('#loading').hide();
            },
            failure: function (response) {
                alert(response.d);
            }
        });
    });

    $("#btnCancel2, #btnCancel3").click(function () {
        $("#popup").show();
    });

    $("#btnNewReport").click(function () {
        $("#drpSortBy option").remove();
        $('#lstColumnSort option').remove();
        $("#hdnReportAction").val('Save');
        $("INPUT[type='checkbox']").attr('checked', false);
        $("#txtReportName").val('');
        $('#spnModelTitle').html('New Report: Customer');

        var radio = $("[id*=rdbOrders] input[value=1]");
        radio.attr("checked", "checked");
        EmptyFilters();
        $("#tblFilterChoices tbody").empty();
        $("#lstFilter").val('Name');
        $("#tblName").fadeIn(200).css("display", "block");
        //                $('#tblBalance input[name="$Balance"][value="rdbAny"]').prop('checked', true);
        //                $("#txtBalance").attr("disabled", "disabled");

        $("#chkMainHeader").attr("checked", true);
        $("#chkDatePrepared").attr("checked", true);
        $("#chkTimePrepared").attr("checked", true);

    });



    $('#tblBalance input[name="$Balance"] + label').click(function () {
        if ($(this).text() != "Any") {
            if ($(this).text() == "=") {
                if ($("#txtBalEqual").val() == '') {
                    $("#trBalance").remove();
                }
                $("#txtBalEqual").attr("disabled", false);
                $("#txtBalGreaterAndEqual").val('');
                $("#txtBalGreaterAndEqual").attr("disabled", true);
                $("#txtBalLessAndEqual").val('');
                $("#txtBalLessAndEqual").attr("disabled", true);
                //                        if ($("#txtBalEqual").val() != '') {
                //                            var replaceOperator = $(".balance").html();
                //                            replaceOperator = replaceOperator.replace(">=", '').replace("&gt;=", '').replace("<=", '').replace("&lt;=", '').replace("=", '');
                //                            $(".balance").html($(this).text() + replaceOperator);
                //                        }
                //                        else {
                //                            $("#trBalance").remove();
                //                        }
            }
            else if ($(this).text() == ">=" || $(this).text() == "&gt;") {
                $("#txtBalGreaterAndEqual").attr("disabled", false);
                $("#txtBalEqual").val('');
                $("#txtBalEqual").attr("disabled", true);
                if ($("#txtBalLessAndEqual").val() == '') {
                    $("#txtBalLessAndEqual").attr("disabled", true);
                }
                else {
                    $("#txtBalLessAndEqual").attr("disabled", false);
                }
                //                        if ($("#txtBalGreaterAndEqual").val() != '') {
                //                            var replaceOperator = $(".balance").html();
                //                            replaceOperator = replaceOperator.replace(">=", '').replace("&gt;=", '').replace("<=", '').replace("&lt;=", '').replace("=", '');
                //                            $(".balance").html($(this).text() + replaceOperator);
                //                        }
                //                        else {
                //                            $("#trBalance").remove();
                //                        }
            }
            else if ($(this).text() == "<=" || $(this).text() == "&lt;") {
                $("#txtBalLessAndEqual").attr("disabled", false);
                $("#txtBalEqual").val('');
                $("#txtBalEqual").attr("disabled", true);
                if ($("#txtBalGreaterAndEqual").val() == '') {
                    $("#txtBalGreaterAndEqual").attr("disabled", true);
                }
                else {
                    $("#txtBalGreaterAndEqual").attr("disabled", false);
                }
                //                        if ($("#txtBalLessAndEqual").val() != '') {
                //                            var replaceOperator = $(".balance").html();
                //                            replaceOperator = replaceOperator.replace(">=", '').replace("&gt;=", '').replace("<=", '').replace("&lt;=", '').replace("=", '');
                //                            $(".balance").html($(this).text() + replaceOperator);
                //                        }
                //                        else {
                //                            $("#trBalance").remove();
                //                        }
            }
        }
        else {
            //                    $("#txtBalance").val('');
            //                    $("#txtBalance").attr("disabled", true);

            $("#txtBalEqual").val('');
            $("#txtBalEqual").attr("disabled", true);
            $("#txtBalLessAndEqual").val('');
            $("#txtBalLessAndEqual").attr("disabled", true);
            $("#txtBalGreaterAndEqual").val('');
            $("#txtBalGreaterAndEqual").attr("disabled", true);
            $("#trBalance").remove();

        }
    });

    $('#tblLoc input[name="$loc"] + label').click(function () {
        if ($(this).text() != "Any") {
            if ($(this).text() == "=") {
                if ($("#txtLocEqual").val() == '') {
                    $("#trloc").remove();
                }
                $("#txtLocEqual").attr("disabled", false);
                $("#txtLocGreatAndEqual").val('');
                $("#txtLocGreatAndEqual").attr("disabled", true);
                $("#txtLocLessAndEqual").val('');
                $("#txtLocLessAndEqual").attr("disabled", true);
            }
            else if ($(this).text() == ">=" || $(this).text() == "&gt;") {
                $("#txtLocGreatAndEqual").attr("disabled", false);
                $("#txtLocEqual").val('');
                $("#txtLocEqual").attr("disabled", true);
                if ($("#txtLocLessAndEqual").val() == '') {
                    $("#txtLocLessAndEqual").attr("disabled", true);
                }
                else {
                    $("#txtLocLessAndEqual").attr("disabled", false);
                }
            }
            else if ($(this).text() == "<=" || $(this).text() == "&lt;") {
                $("#txtLocLessAndEqual").attr("disabled", false);
                $("#txtLocEqual").val('');
                $("#txtLocEqual").attr("disabled", true);
                if ($("#txtLocGreatAndEqual").val() == '') {
                    $("#txtLocGreatAndEqual").attr("disabled", true);
                }
                else {
                    $("#txtLocGreatAndEqual").attr("disabled", false);
                }
            }
        }
        else {
            $("#txtLocEqual").val('');
            $("#txtLocEqual").attr("disabled", true);
            $("#txtLocLessAndEqual").val('');
            $("#txtLocLessAndEqual").attr("disabled", true);
            $("#txtLocGreatAndEqual").val('');
            $("#txtLocGreatAndEqual").attr("disabled", true);
            $("#trloc").remove();

        }
    });

    $('#tblEquip input[name="$equip"] + label').click(function () {
        if ($(this).text() != "Any") {
            if ($(this).text() == "=") {
                if ($("#txtEquipEqual").val() == '') {
                    $("#trequip").remove();
                }
                $("#txtEquipEqual").attr("disabled", false);
                $("#txtEquipGreatAndEqual").val('');
                $("#txtEquipGreatAndEqual").attr("disabled", true);
                $("#txtEquipLessAndEqual").val('');
                $("#txtEquipLessAndEqual").attr("disabled", true);
            }
            else if ($(this).text() == ">=" || $(this).text() == "&gt;") {
                $("#txtEquipGreatAndEqual").attr("disabled", false);
                $("#txtEquipEqual").val('');
                $("#txtEquipEqual").attr("disabled", true);
                if ($("#txtEquipLessAndEqual").val() == '') {
                    $("#txtEquipLessAndEqual").attr("disabled", true);
                }
                else {
                    $("#txtequipLessAndEqual").attr("disabled", false);
                }
            }
            else if ($(this).text() == "<=" || $(this).text() == "&lt;") {
                $("#txtEquipLessAndEqual").attr("disabled", false);
                $("#txtEquipEqual").val('');
                $("#txtEquipEqual").attr("disabled", true);
                if ($("#txtEquipGreatAndEqual").val() == '') {
                    $("#txtEquipGreatAndEqual").attr("disabled", true);
                }
                else {
                    $("#txtEquipGreatAndEqual").attr("disabled", false);
                }
            }
        }
        else {

            $("#txtEquipEqual").val('');
            $("#txtEquipEqual").attr("disabled", true);
            $("#txtEquipLessAndEqual").val('');
            $("#txtEquipLessAndEqual").attr("disabled", true);
            $("#txtEquipGreatAndEqual").val('');
            $("#txtEquipGreatAndEqual").attr("disabled", true);
            $("#trequip").remove();

        }
    });

    //change by Yashasvi Jadav
    $('#tblEquipmentCounts input[name="$equipmentcounts"] + label').click(function () {
        if ($(this).text() != "Any") {
            if ($(this).text() == "=") {
                if ($("#txtEquipmentCountsEqual").val() == '') {
                    $("#trEquipmentCounts").remove();
                }
                $("#txtEquipmentCountsEqual").attr("disabled", false);
                $("#txtEquipmentCountsGreatAndEqual").val('');
                $("#txtEquipmentCountsGreatAndEqual").attr("disabled", true);
                $("#txtEquipmentCountsLessAndEqual").val('');
                $("#txtEquipmentCountsLessAndEqual").attr("disabled", true);
            }
            else if ($(this).text() == ">=" || $(this).text() == "&gt;") {
                $("#txtEquipmentCountsGreatAndEqual").attr("disabled", false);
                $("#txtEquipmentCountsEqual").val('');
                $("#txtEquipmentCountsEqual").attr("disabled", true);
                if ($("#txtEquipmentCountsLessAndEqual").val() == '') {
                    $("#txtEquipmentCountsLessAndEqual").attr("disabled", true);
                }
                else {
                    $("#txtEquipmentCountsLessAndEqual").attr("disabled", false);
                }
            }
            else if ($(this).text() == "<=" || $(this).text() == "&lt;") {
                $("#txtEquipmentCountsLessAndEqual").attr("disabled", false);
                $("#txtEquipmentCountsEqual").val('');
                $("#txtEquipmentCountsEqual").attr("disabled", true);
                if ($("#txtEquipmentCountsGreatAndEqual").val() == '') {
                    $("#txtEquipmentCountsGreatAndEqual").attr("disabled", true);
                }
                else {
                    $("#txtEquipmentCountsGreatAndEqual").attr("disabled", false);
                }
            }
        }
        else {
            $("#txtEquipmentCountsEqual").val('');
            $("#txtEquipmentCountsEqual").attr("disabled", true);
            $("#txtEquipmentCountsLessAndEqual").val('');
            $("#txtEquipmentCountsLessAndEqual").attr("disabled", true);
            $("#txtEquipmentCountsGreatAndEqual").val('');
            $("#txtEquipmentCountsGreatAndEqual").attr("disabled", true);
            $("#trEquipmentCounts").remove();

        }
    });


    $('#tblOpenCalls input[name="$oc"] + label').click(function () {
        if ($(this).text() != "Any") {
            if ($(this).text() == "=") {
                if ($("#txtOCEqual").val() == '') {
                    $("#tropencall").remove();
                }
                $("#txtOCEqual").attr("disabled", false);
                $("#txtOCGreatAndEqual").val('');
                $("#txtOCGreatAndEqual").attr("disabled", true);
                $("#txtOCLessAndEqual").val('');
                $("#txtOCLessAndEqual").attr("disabled", true);
            }
            else if ($(this).text() == ">=" || $(this).text() == "&gt;") {
                $("#txtOCGreatAndEqual").attr("disabled", false);
                $("#txtOCEqual").val('');
                $("#txtOCEqual").attr("disabled", true);
                if ($("#txtOCLessAndEqual").val() == '') {
                    $("#txtOCLessAndEqual").attr("disabled", true);
                }
                else {
                    $("#txtOCLessAndEqual").attr("disabled", false);
                }
            }
            else if ($(this).text() == "<=" || $(this).text() == "&lt;") {
                $("#txtOCLessAndEqual").attr("disabled", false);
                $("#txtOCEqual").val('');
                $("#txtOCEqual").attr("disabled", true);
                if ($("#txtOCGreatAndEqual").val() == '') {
                    $("#txtOCGreatAndEqual").attr("disabled", true);
                }
                else {
                    $("#txtOCGreatAndEqual").attr("disabled", false);
                }
            }
        }
        else {

            $("#txtOCEqual").val('');
            $("#txtOCEqual").attr("disabled", true);
            $("#txtOCLessAndEqual").val('');
            $("#txtOCLessAndEqual").attr("disabled", true);
            $("#txtOCGreatAndEqual").val('');
            $("#txtOCGreatAndEqual").attr("disabled", true);
            $("#tropencall").remove();

        }
    });

    $('#tblEquipmentPrice input[name="$ep"] + label').click(function () {
        if ($(this).text() != "Any") {
            if ($(this).text() == "=") {
                if ($("#txtEquipmentPriceEqual").val() == '') {
                    $("#trEquipmentPrice").remove();
                }
                $("#txtEquipmentPriceEqual").attr("disabled", false);
                $("#txtEquipmentPriceGreatAndEqual").val('');
                $("#txtEquipmentPriceGreatAndEqual").attr("disabled", true);
                $("#txtEquipmentPriceLessAndEqual").val('');
                $("#txtEquipmentPriceLessAndEqual").attr("disabled", true);
            }
            else if ($(this).text() == ">=" || $(this).text() == "&gt;") {
                $("#txtEquipmentPriceGreatAndEqual").attr("disabled", false);
                $("#txtEquipmentPriceEqual").val('');
                $("#txtEquipmentPriceEqual").attr("disabled", true);
                if ($("#txtEquipmentPriceLessAndEqual").val() == '') {
                    $("#txtEquipmentPriceLessAndEqual").attr("disabled", true);
                }
                else {
                    $("#txtEquipmentPriceLessAndEqual").attr("disabled", false);
                }
            }
            else if ($(this).text() == "<=" || $(this).text() == "&lt;") {
                $("#txtEquipmentPriceLessAndEqual").attr("disabled", false);
                $("#txtEquipmentPriceEqual").val('');
                $("#txtEquipmentPriceEqual").attr("disabled", true);
                if ($("#txtEquipmentPriceGreatAndEqual").val() == '') {
                    $("#txtEquipmentPriceGreatAndEqual").attr("disabled", true);
                }
                else {
                    $("#txtEquipmentPriceGreatAndEqual").attr("disabled", false);
                }
            }
        }
        else {

            $("#txtEquipmentPriceEqual").val('');
            $("#txtEquipmentPriceEqual").attr("disabled", true);
            $("#txtEquipmentPriceLessAndEqual").val('');
            $("#txtEquipmentPriceLessAndEqual").attr("disabled", true);
            $("#txtEquipmentPriceGreatAndEqual").val('');
            $("#txtEquipmentPriceGreatAndEqual").attr("disabled", true);
            $("#trEquipmentPrice").remove();
        }
    });

    $('#drpType input[type="checkbox"]').click(function () {
        var filterValue = '';
        $('#drpType input[type=checkbox]:checked + label').each(function () {
            //alert($(this).text());
            filterValue += "'" + $(this).text() + "'" + " | ";
        });
        filterValue = filterValue.substring(0, filterValue.trim().length - 1);

        if (filterValue != '') {
            if ($('#tblFilterChoices tr td').hasClass("type")) {
                $(".type").html(filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trType"><td width="100px" style="height:25px;">Type</td><td  class="type" width="220px"  style="height:25px;">' + filterValue + '</td></tr>');
            }
        }
        else {
            $("#trType").remove();
        }
    });

    $('#drpName input[type="checkbox"]').click(function () {
        var filterValue = '';
        $('#drpName input[type=checkbox]:checked + label').each(function () {
            //alert($(this).text());
            filterValue += "'" + $(this).text() + "'" + " | ";
        });
        filterValue = filterValue.substring(0, filterValue.trim().length - 1);

        if (filterValue != '') {
            if ($('#tblFilterChoices tr td').hasClass("name")) {
                $(".name").html(filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trName"><td width="100px" style="height:25px;">Name</td><td  class="name" width="220px"  style="height:25px;">' + filterValue + '</td></tr>');
            }
        }
        else {
            $("#trName").remove();
        }
    });

    //Changed by Yashasvi Jadav
    $('#drpRoute input[type="checkbox"]').click(function () {
        var filterValue = '';
        $('#drpRoute input[type="checkbox"]:checked + label').each(function () {
            //alert($(this).text());
            filterValue += "'" + $(this).text() + "'" + " | ";
        });
        filterValue = filterValue.substring(0, filterValue.trim().length - 1);

        if (filterValue != '') {
            if ($('#tblFilterChoices tr td').hasClass("route")) {
                $(".route").html(filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trRoute"><td width="100px" style="height:25px;">Route</td><td  class="route" width="220px"  style="height:25px;">' + filterValue + '</td></tr>');
            }
        }
        else {
            $("#trRoute").remove();
        }
    });

    $('#drpLocationSTax input[type="checkbox"]').click(function () {
        var filterValue = '';
        $('#drpLocationSTax input[type="checkbox"]:checked + label').each(function () {
            //alert($(this).text());
            filterValue += "'" + $(this).text() + "'" + " | ";
        });
        filterValue = filterValue.substring(0, filterValue.trim().length - 1);

        if (filterValue != '') {
            if ($('#tblFilterChoices tr td').hasClass("locationstax")) {
                $(".locationstax").html(filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trLocationSTax"><td width="100px" style="height:25px;">LocationSTax</td><td  class="locationstax" width="220px"  style="height:25px;">' + filterValue + '</td></tr>');
            }
        }
        else {
            $("#trLocationSTax").remove();
        }
    });

    $('#drpDefaultSalesPerson input[type="checkbox"]').click(function () {
        var filterValue = '';
        $('#drpDefaultSalesPerson input[type="checkbox"]:checked + label').each(function () {
            //alert($(this).text());
            filterValue += "'" + $(this).text() + "'" + " | ";
        });
        filterValue = filterValue.substring(0, filterValue.trim().length - 1);

        if (filterValue != '') {
            if ($('#tblFilterChoices tr td').hasClass("defaultsalesperson")) {
                $(".defaultsalesperson").html(filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trDefaultSalesPerson"><td width="100px" style="height:25px;">DefaultSalesPerson</td><td  class="defaultsalesperson" width="220px"  style="height:25px;">' + filterValue + '</td></tr>');
            }
        }
        else {
            $("#trDefaultSalesPerson").remove();
        }
    });

    $('#drpEquipmentState input[type="checkbox"]').click(function () {
        var filterValue = '';
        $('#drpEquipmentState input[type="checkbox"]:checked + label').each(function () {
            //alert($(this).text());
            filterValue += "'" + $(this).text() + "'" + " | ";
        });
        filterValue = filterValue.substring(0, filterValue.trim().length - 1);

        if (filterValue != '') {
            if ($('#tblFilterChoices tr td').hasClass("equipmentstate")) {
                $(".equipmentstate").html(filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trEquipmentState"><td width="100px" style="height:25px;">EquipmentState</td><td class="equipmentstate" width="220px" style="height:25px;">' + filterValue + '</td></tr>');
            }
        }
        else {
            $("#trEquipmentState").remove();
        }
    });

    $('#drpBuldingType input[type="checkbox"]').click(function () {
        var filterValue = '';
        $('#drpBuldingType input[type="checkbox"]:checked + label').each(function () {
            //alert($(this).text());
            filterValue += "'" + $(this).text() + "'" + " | ";
        });
        filterValue = filterValue.substring(0, filterValue.trim().length - 1);

        if (filterValue != '') {
            if ($('#tblFilterChoices tr td').hasClass("buildingType")) {
                $(".buildingType").html(filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trBuildingType"><td width="100px" style="height:25px;">Bulding Type</td><td  class="buildingType" width="220px"  style="height:25px;">' + filterValue + '</td></tr>');
            }
        }
        else {
            $("#trBuildingType").remove();
        }
    });

    $('#drpCity input[type="checkbox"]').click(function () {

        var filterValue = '';
        $('#drpCity input[type=checkbox]:checked + label').each(function () {
            //alert($(this).text());
            filterValue += "'" + $(this).text() + "'" + " | ";
        });
        filterValue = filterValue.substring(0, filterValue.trim().length - 1);

        if (filterValue != '') {
            if ($('#tblFilterChoices tr td').hasClass("city")) {
                $(".city").html(filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trCity"><td width="100px" style="height:25px;">City</td><td  class="city" width="220px"  style="height:25px;">' + filterValue + '</td></tr>');
            }
        }
        else {
            $("#trCity").remove();
        }
    });

    $('#drpAddress input[type="checkbox"]').click(function () {
        var filterValue = '';
        $('#drpAddress input[type=checkbox]:checked + label').each(function () {
            //alert($(this).text());
            filterValue += "'" + $(this).text() + "'" + " | ";
        });
        filterValue = filterValue.substring(0, filterValue.trim().length - 1);

        if (filterValue != '') {
            if ($('#tblFilterChoices tr td').hasClass("address")) {
                $(".address").html(filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trAddress"><td width="100px" style="height:25px;">Address</td><td  class="address" width="220px"  style="height:25px;">' + filterValue + '</td></tr>');
            }
        }
        else {
            $("#trAddress").remove();
        }
    });

    $('#ddlState input[type="checkbox"]').click(function () {
        var filterValue = '';
        $('#ddlState input[type=checkbox]:checked + label').each(function () {
            //alert($(this).text());
            filterValue += "'" + $(this).text() + "'" + " | ";
        });
        filterValue = filterValue.substring(0, filterValue.trim().length - 1);

        if (filterValue != '') {
            if ($('#tblFilterChoices tr td').hasClass("state")) {
                $(".state").html(filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trState"><td width="100px" style="height:25px;">State</td><td  class="state" width="220px"  style="height:25px;">' + filterValue + '</td></tr>');
            }
        }
        else {
            $("#trState").remove();
        }
    });

    function SetFiltersChoicesFromDropDown(ControlId, cls) {
        cls = cls.replace(" ", "");
        var filterValue = '';
        debugger;
        $(' ' + ControlId + ' input[type=checkbox]:checked + label').each(function () {
            filterValue += "'" + $(this).text() + "'" + " | ";
        });
        filterValue = filterValue.substring(0, filterValue.trim().length - 1);

        if (filterValue != '') {
            if ($('#tblFilterChoices tr td').hasClass(cls)) {
                $("." + cls).html(filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="tr' + cls + '"><td width="100px" style="height:25px;">' + cls + '</td><td  class=' + cls + ' width="220px"  style="height:25px;">' + filterValue + '</td></tr>');
            }
        }
        else {
            $("#tr" + cls + "").remove();
        }
    }

    $('#drpCustomer input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#drpCustomer", "Customer");
    });

    $('#drpLocationId input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#drpLocationId", "Location Id");
    });

    //Rahil
    $('#drpLocationName input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#drpLocationName", "Location");
    });

    $('#drpLocationType input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#drpLocationType", "Loc Type");
    });

    $('#drpDescription input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#drpDescription", "Description");
    });

    $('#drpTicketTime input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#drpTicketTime", "Ticket Time");
    });

    $('#drpServiceType input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#drpServiceType", "Service Type");
    });

    $('#drpTicketStart input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#drpTicketStart", "Ticket Start");
    });

    $('#drpHours input[type="checkbox"]').click(function () {      
        SetFiltersChoicesFromDropDown("#drpHours", "Hours");
    });

    $('#drpTicketFreq input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#drpTicketFreq", "Ticket Freq");
    });

    $('#drpPreferredWorker input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#drpPreferredWorker", "Preferred Worker");
    });

    $('#drpBillAmount input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#drpBillAmount", "Bill Amount");
    });

    $('#drpEquipment input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#drpEquipment", "Equipment");
    });

    $('#drpExpiration input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#drpExpiration", "Expiration");
    });

    $('#drpExpirationDate input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#drpExpirationDate", "Expiration Date");
    });

    $('#drpPhoneMonitoring input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#drpPhoneMonitoring", "Phone Monitoring");
    });

    $('#drpContractType input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#drpContractType", "Contract Type");
    });

    $('#drpOccupancyDiscount input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#drpOccupancyDiscount", "Occupancy Discount");
    });

    $('#drpExclusions input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#drpExclusions", "Exclusions");
    });

    $('#drpTermOfContract input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#drpTermOfContract", "Term Of Contract");
    });

    $('#drpPriceAdjustmentCap input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#drpPriceAdjustmentCap", "Price Adjustment Cap");
    });

    $('#drpFireServiceTestingIncluded input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#drpFireServiceTestingIncluded", "Fire Service Testing Included");
    });

    $('#drpSpecialRates input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#drpSpecialRates", "Special Rates");
    });

    $('#drpContractExpiration input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#drpContractExpiration", "Contract Expiration");
    });

    $('#drpProratedItems input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#drpProratedItems", "Prorated Items");
    });

    $('#drpAnnualTestIncluded input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#drpAnnualTestIncluded", "Annual Test Included");
    });

    $('#drpFiveYearStateTestIncluded input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#drpFiveYearStateTestIncluded", "Five Year State Test Included");
    });

    $('#drpFireServiceTestedIncluded input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#drpFireServiceTestedIncluded", "Fire Service Tested Included");
    });

    $('#drpCancellationNotificationDays input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#drpCancellationNotificationDays", "Cancellation Notification Days");
    });

    $('#drpPriceAdjustmentNotificationDays input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#drpPriceAdjustmentNotificationDays", "Price Adjustment Notification Days");
    });

    $('#drpAfterHoursCallsIncluded input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#drpAfterHoursCallsIncluded", "After Hours Calls Included");
    });

    $('#drpOGServiceCallsIncluded input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#drpOGServiceCallsIncluded", "OG Service Calls Included");
    });

    $('#drpContractHours input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#drpContractHours", "Contract Hours");
    });

    $('#drpContractFormat input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#drpContractFormat", "Contract Format");
    });

    $('#drpBillFreqency input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#drpBillFreqency", "BillStart");
    });

    $("#btnCustomizeReport").click(function () {
        debugger;
        $("#chkColumnList INPUT[type='checkbox']").attr('checked', false);
        // var reportValue = $("#drpReports option:selected").text();
        $("#hdnReportAction").val('Update');
        var reportName = $("#hdnCustomizeReportName").val();
        $('#spnModelTitle').html('Modify Report : ' + reportName);
        $("#txtReportName").val(reportName);
        var columnList = $("#hdnColumnList").val();

        var splitedColList = columnList.split(",");
        for (i = 0; i <= splitedColList.length; i++) {
            $('#chkColumnList input[type=checkbox]').each(function () {
                var lblValue = $(this).parent().find('label').html();
                if (lblValue == splitedColList[i]) {
                    $(this).attr('checked', true);
                }
            });
        }

        $("#drpSortBy option").remove();
        $("#lstColumnSort option").remove();

        $.each(splitedColList, function (key, value) {

            $('#drpSortBy')
         .append($("<option></option>")
         .attr("value", value)
         .text(value));


            $('#lstColumnSort')
                                            .append($("<option></option>")
                                 .attr("value", value)
                                 .text(value));

        });

        if ($('#tblResize tr').length > 0) {
            $('#lstColumnSort option').remove();
            $('#tblResize tr th').each(function () {
                $('#lstColumnSort')
                                    .append($("<option></option>")
                         .attr("value", $(this).html())
                         .text($(this).html()));
            });
        }


        debugger;
        $('#drpSortBy').val($("#hdnDrpSortBy").val());
        if (filters.length == 0) {
            $("#tblFilterChoices tbody").empty();
            $("#lstFilter").val('Name');
            EmptyFilters();
            $("#tblName").fadeIn(200).css("display", "block");
        }
        else {
            $("#tblFilterChoices tbody").empty();

            $(filters).each(function (index, filter) {
                if (filter.FilterValues != '') {
                    if (filter.FilterColumns != "State" && filter.FilterColumns != "LocationState") {
                        $('#tblFilterChoices tbody').append('<tr  id="tr' + filter.FilterColumns + '"><td width="100px" style="height:25px;">' + filter.FilterColumns + '</td><td  class="' + filter.FilterColumns.toLowerCase() + '" width="220px"  style="height:25px;">' + filter.FilterValues + '</td></tr>');
                    }
                    else {
                        var getStates = filter.FilterValues;
                        var splitedStates = getStates.split("|");
                        var getText = '';
                        for (i = 0; i < splitedStates.length; i++) {
                            actualText = $.trim(splitedStates[i].replace("'", "").replace("'", "").replace(",", ""));
                            $("#ddlStateReference option[value= " + actualText + "]").attr("selected", "selected")
                            getText += "'" + $("#ddlStateReference option:selected").text() + "'" + " | ";

                        }
                        if (filter.FilterColumns == "State") {
                            $('#tblFilterChoices tbody').append('<tr  id="tr' + filter.FilterColumns + '"><td width="100px" style="height:25px;">' + filter.FilterColumns + '</td><td  class="' + filter.FilterColumns.toLowerCase() + '" width="220px"  style="height:25px;">' + getText.trim().substring(0, getText.trim().length - 1) + '</td></tr>');
                        }
                        else {
                            $('#tblFilterChoices tbody').append('<tr  id="tr' + filter.FilterColumns + '"><td width="100px" style="height:25px;">' + filter.FilterColumns + '</td><td  class="' + filter.FilterColumns + '" width="220px"  style="height:25px;">' + getText.trim().substring(0, getText.trim().length - 1) + '</td></tr>');
                        }
                        //alert(getText);
                    }
                    setFiltersValue(filter.FilterColumns, filter.FilterValues);
                }
            });

            $('#tblFilterChoices tr:first').each(function () {
                filterColumns = $(this).find("td:first").html()
                setFilters(filterColumns);
                $("#lstFilter").val(filterColumns);
            });
        }
    });

    $('#chkColumnList').change(function () {
        $("#drpSortBy option").remove();
        $('#lstColumnSort option').remove();
        //  var lstColumnValues = '';
        $('#chkColumnList input[type=checkbox]').each(function () {
            if ($(this).is(":checked")) {
                $('#drpSortBy').append($("<option></option>").attr("value", $(this).parent().find('label').html()).text($(this).parent().find('label').html()));

                $('#lstColumnSort').append($("<option></option>").attr("value", $(this).parent().find('label').html()).text($(this).parent().find('label').html()));

            }

        });

    });


    $("#lstFilter").change(function () {
        var filterName = $("#lstFilter option:selected").text();

        setFilters(filterName);
    });

    $('#tblFilterChoices').on('click', 'tr', function (event) {
        if ($(this).attr('style'))
            $(this).removeAttr('style');

        var selected = $(this).hasClass("highlight");

        $("#tblFilterChoices tr").removeClass("highlight");
        if (!selected) {
            $(this).addClass("highlight");
        }

        var filterName = $(this).find("td:first").html();
        var filterValue = $(this).find("td:last").html();
        //EmptyFilters();
        setFilters(filterName);
        setFiltersValue(filterName, filterValue);
        $("#lstFilter").val(filterName);
    });

    $("#btnRemoveFilter").click(function () {
        if ($('#tblFilterChoices tr').length > 0) {
            var checkSelected = false;
            $('#tblFilterChoices tr').each(function () {
                var selected = $(this).hasClass("highlight");
                if (selected) {
                    var filterName = $(this).find("td:first").html();
                    $(this).remove();
                    setFiltersValue(filterName, "");
                    checkSelected = true;
                }
            });
            if (!checkSelected) {
                alert("Please select filter value to remove.");
            }
        }
        else {
            alert("No filter value available to remove.");
        }
    });

    $("#txtZip").blur(function () {
        var filterValue = $("#txtZip").val();
        if (filterValue != '') {
            if ($('#tblFilterChoices tr td').hasClass("zip")) {
                $(".zip").html(filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trZip"><td width="100px" style="height:25px;">Zip</td><td  class="zip" width="220px"  style="height:25px;">' + filterValue + '</td></tr>');
            }
        }
        else {
            $("#trZip").remove();
        }
    });

    $("#txtInstalledOn").blur(function () {
        var filterValue = $("#txtInstalledOn").val();
        if (filterValue != '') {
            if ($('#tblFilterChoices tr td').hasClass("installedon")) {
                $(".installedon").html(filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trInstalledOn"><td width="100px" style="height:25px;">InstalledOn</td><td  class="installedon" width="220px"  style="height:25px;">' + filterValue + '</td></tr>');
            }
        }
        else {
            $("#trZip").remove();
        }
    });

    $("#txtPhone").blur(function () {
        var filterValue = $("#txtPhone").val();
        if (filterValue != '') {
            if ($('#tblFilterChoices tr td').hasClass("phone")) {
                $(".phone").html(filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trPhone"><td width="100px" style="height:25px;">Phone</td><td  class="phone" width="220px"  style="height:25px;">' + filterValue + '</td></tr>');
            }
        }
        else {
            $("#trPhone").remove();
        }
    });

    $("#txtFax").blur(function () {
        var filterValue = $("#txtFax").val();
        if (filterValue != '') {
            if ($('#tblFilterChoices tr td').hasClass("fax")) {
                $(".fax").html(filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trFax"><td width="100px" style="height:25px;">Fax</td><td  class="fax" width="220px"  style="height:25px;">' + filterValue + '</td></tr>');
            }
        }
        else {
            $("#trFax").remove();
        }
    });

    $("#txtContact").blur(function () {
        var filterValue = $("#txtContact").val();
        if (filterValue != '') {
            if ($('#tblFilterChoices tr td').hasClass("contact")) {
                $(".contact").html(filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trContact"><td width="100px" style="height:25px;">Contact</td><td  class="contact" width="220px"  style="height:25px;">' + filterValue + '</td></tr>');
            }
        }
        else {
            $("#trContact").remove();
        }
    });

    $("#txtEmail").blur(function () {
        var filterValue = $("#txtEmail").val();
        if (filterValue != '') {
            if ($('#tblFilterChoices tr td').hasClass("email")) {
                $(".email").html(filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trEmail"><td width="100px" style="height:25px;">Email</td><td  class="email" width="220px"  style="height:25px;">' + filterValue + '</td></tr>');
            }
        }
        else {
            $("#trEmail").remove();
        }
    });

    $("#txtCountry").blur(function () {
        var filterValue = $("#txtCountry").val();
        if (filterValue != '') {
            if ($('#tblFilterChoices tr td').hasClass("country")) {
                $(".country").html(filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trCountry"><td width="100px" style="height:25px;">Country</td><td  class="country" width="220px"  style="height:25px;">' + filterValue + '</td></tr>');
            }
        }
        else {
            $("#trCountry").remove();
        }
    });

    $("#txtWebsite").blur(function () {
        var filterValue = $("#txtWebsite").val();
        if (filterValue != '') {
            if ($('#tblFilterChoices tr td').hasClass("website")) {
                $(".website").html(filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trWebsite"><td width="100px" style="height:25px;">Website</td><td  class="website" width="220px"  style="height:25px;">' + filterValue + '</td></tr>');
            }
        }
        else {
            $("#trWebsite").remove();
        }
    });

    $("#txtCellular").blur(function () {
        var filterValue = $("#txtCellular").val();
        if (filterValue != '') {
            if ($('#tblFilterChoices tr td').hasClass("cellular")) {
                $(".cellular").html(filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trCellular"><td width="100px" style="height:25px;">Cellular</td><td  class="cellular" width="220px"  style="height:25px;">' + filterValue + '</td></tr>');
            }
        }
        else {
            $("#trCellular").remove();
        }
    });

    $("#drpCategory").change(function () {
        var filterValue = $("#drpCategory option:selected").text();
        if (filterValue != 'All') {
            if ($('#tblFilterChoices tr td').hasClass("category")) {
                $(".category").html(filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trCategory"><td width="100px" style="height:25px;">Category</td><td  class="category" width="220px"  style="height:25px;">' + filterValue + '</td></tr>');
            }
        }
        else {
            $("#trCategory").remove();
        }
    });


    $("#txtBalEqual").blur(function () {
        var filterValue = $("#txtBalEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblBalance input[name='$Balance']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("balance")) {
                $(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trBalance"><td width="100px" style="height:25px;">Balance</td><td  class="balance" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        //                else {
        //                    $("#trBalance").remove();
        //                }
    });

    $("#txtBalGreaterAndEqual").blur(function () {
        var filterValue = $("#txtBalGreaterAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblBalance input[name='$Balance']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("balance") && $("#txtBalLessAndEqual").val() != '') {
                getLessValue = "<=" + $("#txtBalLessAndEqual").val();
                $(".balance").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getLessValue);
                $(".balance").html(">=" + filterValue + " and " + getLessValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("balance") && $("#txtBalLessAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".balance").html(">=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trBalance"><td width="100px" style="height:25px;">Balance</td><td  class="balance" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#txtBalLessAndEqual").val() == '') {
                $("#trBalance").remove();
            }
            else {
                $(".balance").html("<=" + $("#txtBalLessAndEqual").val());
            }
        }
    });

    $("#txtBalLessAndEqual").blur(function () {
        var filterValue = $("#txtBalLessAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblBalance input[name='$Balance']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("balance") && $("#txtBalGreaterAndEqual").val() != '') {
                getGreaterValue = ">=" + $("#txtBalGreaterAndEqual").val()
                $(".balance").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getGreaterValue);
                $(".balance").html(getGreaterValue + " and " + "<=" + filterValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("balance") && $("#txtBalGreaterAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".balance").html("<=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trBalance"><td width="100px" style="height:25px;">Balance</td><td  class="balance" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#txtBalGreaterAndEqual").val() == '') {
                $("#trBalance").remove();
            }
            else {
                $(".balance").html(">=" + $("#txtBalGreaterAndEqual").val());
            }
        }
    });


    $("#drpStatus").change(function () {
        var filterValue = $("#drpStatus option:selected").text();
        if (filterValue != 'All') {
            if ($('#tblFilterChoices tr td').hasClass("status")) {
                $(".status").html(filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trStatus"><td width="100px" style="height:25px;">Status</td><td  class="status" width="220px"  style="height:25px;">' + filterValue + '</td></tr>');
            }
        }
        else {
            $("#trStatus").remove();
        }
    });


    $("#txtLocEqual").blur(function () {
        var filterValue = $("#txtLocEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblLoc input[name='$loc']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("loc")) {
                $(".loc").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trloc"><td width="100px" style="height:25px;">loc</td><td  class="loc" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        //                else {
        //                    $("#trBalance").remove();
        //                }
    });

    $("#txtLocGreatAndEqual").blur(function () {
        var filterValue = $("#txtLocGreatAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblLoc input[name='$loc']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("loc") && $("#txtLocLessAndEqual").val() != '') {
                getLessValue = "<=" + $("#txtLocLessAndEqual").val();
                $(".loc").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getLessValue);
                $(".loc").html(">=" + filterValue + " and " + getLessValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("loc") && $("#txtLocLessAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".loc").html(">=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trloc"><td width="100px" style="height:25px;">loc</td><td  class="loc" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#txtLocLessAndEqual").val() == '') {
                $("#trloc").remove();
            }
            else {
                $(".loc").html("<=" + $("#txtLocLessAndEqual").val());
            }
        }
    });

    $("#txtLocLessAndEqual").blur(function () {
        var filterValue = $("#txtLocLessAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblLoc input[name='$loc']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("loc") && $("#txtLocGreatAndEqual").val() != '') {
                getGreaterValue = ">=" + $("#txtLocGreatAndEqual").val()
                $(".loc").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getGreaterValue);
                $(".loc").html(getGreaterValue + " and " + "<=" + filterValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("loc") && $("#txtLocGreatAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".loc").html("<=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trloc"><td width="100px" style="height:25px;">loc</td><td  class="loc" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#txtLocGreatAndEqual").val() == '') {
                $("#trloc").remove();
            }
            else {
                $(".loc").html(">=" + $("#txtLocGreatAndEqual").val());
            }
        }
    });




    $("#txtEquipEqual").blur(function () {
        var filterValue = $("#txtEquipEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblEquip input[name='$equip']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("equip")) {
                $(".equip").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trequip"><td width="100px" style="height:25px;">equip</td><td  class="equip" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        //                else {
        //                    $("#trBalance").remove();
        //                }
    });


    $("#txtEquipGreatAndEqual").blur(function () {
        var filterValue = $("#txtEquipGreatAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblEquip input[name='$equip']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("equip") && $("#txtEquipLessAndEqual").val() != '') {
                getLessValue = "<=" + $("#txtEquipLessAndEqual").val();
                $(".equip").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getLessValue);
                $(".equip").html(">=" + filterValue + " and " + getLessValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("equip") && $("#txtEquipLessAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".equip").html(">=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trequip"><td width="100px" style="height:25px;">equip</td><td  class="equip" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#txtEquipLessAndEqual").val() == '') {
                $("#trequip").remove();
            }
            else {
                $(".equip").html("<=" + $("#txtEquipLessAndEqual").val());
            }
        }
    });

    $("#txtEquipLessAndEqual").blur(function () {
        var filterValue = $("#txtEquipLessAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblEquip input[name='$equip']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("equip") && $("#txtEquipGreatAndEqual").val() != '') {
                getGreaterValue = ">=" + $("#txtEquipGreatAndEqual").val()
                $(".equip").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getGreaterValue);
                $(".equip").html(getGreaterValue + " and " + "<=" + filterValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("equip") && $("#txtEquipGreatAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".equip").html("<=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trequip"><td width="100px" style="height:25px;">equip</td><td  class="equip" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#txtEquipGreatAndEqual").val() == '') {
                $("#trequip").remove();
            }
            else {
                $(".equip").html(">=" + $("#txtEquipGreatAndEqual").val());
            }
        }
    });


    //Added by Yashasvi Jadav
    $("#txtEquipmentCountsEqual").blur(function () {
        var filterValue = $("#txtEquipmentCountsEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblEquipmentCounts input[name='$equipmentcounts']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("equipmentcounts")) {
                $(".equipmentcounts").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trEquipmentCounts"><td width="100px" style="height:25px;">EquipmentCounts</td><td  class="equipmentcounts" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        //                else {
        //                    $("#trBalance").remove();
        //                }
    });


    $("#txtEquipmentCountsGreatAndEqual").blur(function () {
        var filterValue = $("#txtEquipmentCountsGreatAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblEquipmentCounts input[name='$equipmentcounts']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("equipmentcounts") && $("#txtEquipmentCountsLessAndEqual").val() != '') {
                getLessValue = "<=" + $("#txtEquipmentCountsLessAndEqual").val();
                $(".equipmentcounts").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getLessValue);
                $(".equipmentcounts").html(">=" + filterValue + " and " + getLessValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("equipmentcounts") && $("#txtEquipmentCountsLessAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".equipmentcounts").html(">=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trEquipmentCounts"><td width="100px" style="height:25px;">EquipmentCounts</td><td  class="equipmentcounts" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#txtEquipmentCountsLessAndEqual").val() == '') {
                $("#trEquipmentCounts").remove();
            }
            else {
                $(".equipmentcounts").html("<=" + $("#txtEquipmentCountsLessAndEqual").val());
            }
        }
    });

    $("#txtEquipmentCountsLessAndEqual").blur(function () {
        var filterValue = $("#txtEquipmentCountsLessAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblEquipmentCounts input[name='$equipmentCounts']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("equipmentcounts") && $("#txtEquipmentCountsGreatAndEqual").val() != '') {
                getGreaterValue = ">=" + $("#txtEquipmentCountsGreatAndEqual").val()
                $(".equipmentcounts").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getGreaterValue);
                $(".equipmentcounts").html(getGreaterValue + " and " + "<=" + filterValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("equip") && $("#txtEquipmentcountsGreatAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".equipmentcounts").html("<=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trEquipmentCounts"><td width="100px" style="height:25px;">EquipmentCounts</td><td  class="equip" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#txtEquipmentCountsGreatAndEqual").val() == '') {
                $("#trequip").remove();
            }
            else {
                $(".equipmentcounts").html(">=" + $("#txtEquipmentCountsGreatAndEqual").val());
            }
        }
    });



    $("#txtOCEqual").blur(function () {
        var filterValue = $("#txtOCEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblOpenCalls input[name='$oc']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("opencall")) {
                $(".opencall").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="tropencall"><td width="100px" style="height:25px;">opencall</td><td  class="opencall" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        //                else {
        //                    $("#trBalance").remove();
        //                }
    });




    $("#txtOCGreatAndEqual").blur(function () {
        var filterValue = $("#txtOCGreatAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblOpenCalls input[name='$oc']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("opencall") && $("#txtOCLessAndEqual").val() != '') {
                getLessValue = "<=" + $("#txtOCLessAndEqual").val();
                $(".opencall").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getLessValue);
                $(".opencall").html(">=" + filterValue + " and " + getLessValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("opencall") && $("#txtOCLessAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".opencall").html(">=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="tropencall"><td width="100px" style="height:25px;">opencall</td><td  class="opencall" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#txtOCLessAndEqual").val() == '') {
                $("#tropencall").remove();
            }
            else {
                $(".opencall").html("<=" + $("#txtOCLessAndEqual").val());
            }
        }
    });

    $("#txtOCLessAndEqual").blur(function () {
        debugger;
        var filterValue = $("#txtOCLessAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblOpenCalls input[name='$oc']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("opencall") && $("#txtOCGreatAndEqual").val() != '') {
                getGreaterValue = ">=" + $("#txtOCGreatAndEqual").val()
                $(".opencall").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getGreaterValue);
                $(".opencall").html(getGreaterValue + " and " + "<=" + filterValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("opencall") && $("#txtOCGreatAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".opencall").html("<=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="tropencall"><td width="100px" style="height:25px;">opencall</td><td  class="opencall" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#txtOCGreatAndEqual").val() == '') {
                $("#tropencall").remove();
            }
            else {
                $(".opencall").html(">=" + $("#txtOCGreatAndEqual").val());
            }
        }
    });




    $("#txtEquipmentPriceEqual").blur(function () {
        var filterValue = $("#txtEquipmentPriceEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblEquipmentPrice input[name='$ep']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("equipmentprice")) {
                $(".equipmentprice").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trEquipmentPrice"><td width="100px" style="height:25px;">EquipmentPrice</td><td  class="equipmentprice" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        //                else {
        //                    $("#trBalance").remove();
        //                }
    });




    $("#txtEquipmentPriceGreatAndEqual").blur(function () {
        var filterValue = $("#txtEquipmentPriceGreatAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblEquipmentPrice input[name='$ep']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("equipmentprice") && $("#txtEquipmentPriceLessAndEqual").val() != '') {
                getLessValue = "<=" + $("#txtEquipmentPriceLessAndEqual").val();
                $(".equipmentprice").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getLessValue);
                $(".equipmentprice").html(">=" + filterValue + " and " + getLessValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("equipmentprice") && $("#txtEquipmentPriceLessAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".equipmentprice").html(">=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trEquipmentPrice"><td width="100px" style="height:25px;">EquipmentPrice</td><td  class="equipmentprice" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#txtEquipmentPriceLessAndEqual").val() == '') {
                $("#trEquipmentPrice").remove();
            }
            else {
                $(".equipmentprice").html("<=" + $("#txtEquipmentPriceLessAndEqual").val());
            }
        }
    });

    $("#txtEquipmentPriceLessAndEqual").blur(function () {
        var filterValue = $("#txtEquipmentPriceLessAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblEquipmentPrice input[name='$ep']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("equipmentprice") && $("#txtEquipmentPriceGreatAndEqual").val() != '') {
                getGreaterValue = ">=" + $("#txtEquipmentPriceGreatAndEqual").val()
                $(".equipmentprice").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getGreaterValue);
                $(".equipmentprice").html(getGreaterValue + " and " + "<=" + filterValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("equipmentprice") && $("#txtEquipmentPriceGreatAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".equipmentprice").html("<=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trEquipmentPrice"><td width="100px" style="height:25px;">EquipmentPrice</td><td  class="equipmentprice" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#txtEquipmentPriceGreatAndEqual").val() == '') {
                $("#trEquipmentPrice").remove();
            }
            else {
                $(".equipmentprice").html(">=" + $("#txtEquipmentPriceGreatAndEqual").val());
            }
        }
    });

    //    $("#txtLocationZip").blur(function() {
    //        SetFiltersChoicesFromTextBox("#txtLocationZip", "LocationZip");
    //    });

    //    function SetFiltersChoicesFromTextBox(ControlId, cls) {
    //        var filterValue = $(ControlId).val();
    //        if (filterValue != '') {
    //            if ($('#tblFilterChoices tr td').hasClass(cls)) {
    //                $("." + cls).html(filterValue);
    //            }
    //            else {
    //                $('#tblFilterChoices tbody').append('<tr id="tr' + cls + '"><td width="100px" style="height:25px;">' + cls + '</td><td  class=' + cls + ' width="220px"  style="height:25px;">' + filterValue + '</td></tr>');
    //            }
    //        }
    //        else {
    //            $("#tr" + cls + "").remove();
    //        }
    //    }


    /////////////////////// Header/Footer /////////////////////////
    $("#chkCompanyName").click(function () {
        if ($(this).is(":checked")) {
            $("#txtCompanyName").attr("disabled", false);
        }
        else {
            $("#txtCompanyName").attr("disabled", true);
            $("#txtCompanyName").val('');
        }
    });

    $("#chkReportTitle").click(function () {
        if ($(this).is(":checked")) {
            $("#txtReportTitle").attr("disabled", false);

        }
        else {
            $("#txtReportTitle").attr("disabled", true);
            $("#txtReportTitle").val('');
        }
    });


    $("#chkSubtitle").click(function () {
        if ($(this).is(":checked")) {
            $("#txtSubtitle").attr("disabled", false);

        }
        else {
            $("#txtSubtitle").attr("disabled", true);
            $("#txtSubtitle").val('');
        }
    });

    $("#chkDatePrepared").click(function () {
        if ($(this).is(":checked")) {
            $("#drpDatePrepared").attr("disabled", false);
        }
        else {
            $("#drpDatePrepared").attr("disabled", true);
        }
    });


    $("#chkPageNumber").click(function () {
        if ($(this).is(":checked")) {
            $("#drpPageNumber").attr("disabled", false);
        }
        else {
            $("#drpPageNumber").attr("disabled", true);
        }
    });

    $("#chkExtraFootLine").click(function () {
        if ($(this).is(":checked")) {
            $("#txtExtraFooterLine").attr("disabled", false);

        }
        else {
            $("#txtExtraFooterLine").attr("disabled", true);
            $("#txtExtraFooterLine").val('');
        }
    });

});

