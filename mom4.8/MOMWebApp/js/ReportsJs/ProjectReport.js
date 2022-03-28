


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
        $('#ctl00_ContentPlaceHolder1_lstColumnSort :selected').each(function (i, selected) {
            if (!$(this).prev().length) return false;
            $(this).insertBefore($(this).prev());
        });
        $('#ctl00_ContentPlaceHolder1_lstColumnSort select').focus().blur();

    });
});

//function moveDown() {
$(function () {
    $("#MoveDown").click(function (event) {
        $($('#ctl00_ContentPlaceHolder1_lstColumnSort :selected').get().reverse()).each(function (i, selected) {
            if (!$(this).next().length) return false;
            $(this).insertAfter($(this).next());
        });
        $('#ctl00_ContentPlaceHolder1_lstColumnSort select').focus().blur();

    });
});
$(function () {
    $("#ReadAll").click(function (event) {
        $('#ctl00_ContentPlaceHolder1_lstColumnSort option').each(function (index) {
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
        $('#ctl00_ContentPlaceHolder1_lstColumnSort :selected').each(function (i, selected) {

            $(this).remove();
            alert('Selected item deleted');
            $('#ctl00_ContentPlaceHolder1_lstColumnSort').focus().blur();

        });
    });
});


function UserDeleteConfirmation() {
    return confirm("Are you sure you want to delete this report?");
}

function EmptyReportName() {
    debugger;
    var _reportName = $('#ctl00_ContentPlaceHolder1_txtReportName').val();
    var _hdnName = $("#ctl00_ContentPlaceHolder1_hdnCustomizeReportName").val();
    var _isStock = $("#ctl00_ContentPlaceHolder1_hdnIsStock").val();

    if ($("#ctl00_ContentPlaceHolder1_txtReportName").val() == "") {
        alert("Report name cann't be empty.");
        return false;
    }
    else {
        $("#divInfo").hide();
        if (_reportName != _hdnName && _isStock == "True") {
            $("#ctl00_ContentPlaceHolder1_hdnReportAction").val('Save');
            return true;
        }
        else if (_reportName == _hdnName && _isStock == "True") {
            alert("You don't have permission to update this report. Please choose another title.");
            $("#ctl00_ContentPlaceHolder1_divInfo").css('display', 'block');
            return false;
        }
        else {
            //$("#ctl00_ContentPlaceHolder1_hdnReportAction").val('Save');
            return true;
        }
    }
}

//function EmptyEmailBox() {
//    if ($("#ctl00_ContentPlaceHolder1_txtEmails").val() == "") {
//        alert("Report name cann't be empty.");
//        return false;
//    }
//    else {
//        return true;
//    }
//}

function EmptyFilters() {
    $("#tblName").css("display", "none");
    //$("#ctl00_ContentPlaceHolder1_drpName").val('All');
    $('#ctl00_ContentPlaceHolder1_drpName input[type="checkbox"]').attr('checked', false);
    $("#tblCity").css("display", "none");
    //            $("#ctl00_ContentPlaceHolder1_txtCity").val('');
    $('#ctl00_ContentPlaceHolder1_drpCity input[type="checkbox"]').attr('checked', false);
    $("#tblState").css("display", "none");

    //Changed by Yashasvi Jadav
    $("#tblRoute").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpRoute input[type="checkbox"]').attr('checked', false);
    $("#tblLocationSTax").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpLocationSTax input[type="checkbox"]').attr('checked', false);
    $("#tblDefaultSalesPerson").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpDefaultSalesPerson input[type="checkbox"]').attr('checked', false);
    $("#tblInstalledOn").css("display", "none");
    $("#ctl00_ContentPlaceHolder1_txtInstalledOn").val('');
    $("#drpBuldingType").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpBuldingType input[type="checkbox"]').attr('checked', false);
    $("#drpEquipmentState").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpEquipmentState input[type="checkbox"]').attr('checked', false);
    //$("#ctl00_ContentPlaceHolder1_ddlState").val('All');
    $('#ctl00_ContentPlaceHolder1_ddlState input[type="checkbox"]').attr('checked', false);
    $("#tblZip").css("display", "none");
    $("#ctl00_ContentPlaceHolder1_txtZip").val('');
    $("#ctl00_ContentPlaceHolder1_txtInstalledOn").val('');
    $("#tblPhone").css("display", "none");
    $("#ctl00_ContentPlaceHolder1_txtPhone").val('');
    $("#tblFax").css("display", "none");
    $("#ctl00_ContentPlaceHolder1_txtFax").val('');
    $("#tblContact").css("display", "none");
    $("#ctl00_ContentPlaceHolder1_txtContact").val('');
    $("#tblAddress").css("display", "none");
    //            $("#ctl00_ContentPlaceHolder1_txtAddress").val('');
    $('#ctl00_ContentPlaceHolder1_drpAddress input[type="checkbox"]').attr('checked', false);
    $("#tblEmail").css("display", "none");
    $("#ctl00_ContentPlaceHolder1_txtEmail").val('');
    $("#tblCountry").css("display", "none");
    $("#ctl00_ContentPlaceHolder1_txtCountry").val('');
    $("#tblWebsite").css("display", "none");
    $("#ctl00_ContentPlaceHolder1_txtWebsite").val('');
    $("#tblCellular").css("display", "none");
    $("#ctl00_ContentPlaceHolder1_txtCellular").val('');
    $("#tblCategory").css("display", "none");
    $("#ctl00_ContentPlaceHolder1_drpCategory").val('Category');
    $("#tblType").css("display", "none");
    // $("#ctl00_ContentPlaceHolder1_drpType").val('All');
    //$('#ctl00_ContentPlaceHolder1_drpType input[type="checkbox"]').attr('checked', false);

    $("#tblBalance").css("display", "none");
    $("#ctl00_ContentPlaceHolder1_txtBalEqual").val('');
    $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").val('');
    $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").val('');

    //Expenses
    $("#tblExpenses").css("display", "none");
    $("#ctl00_ContentPlaceHolder1_txtExpEqual").val('');
    $("#ctl00_ContentPlaceHolder1_txtExpLessAndEqual").val('');
    $("#ctl00_ContentPlaceHolder1_txtExpGreaterAndEqual").val('');

    $("#tblLaborExpense").css("display", "none");
    $("#ctl00_ContentPlaceHolder1_txtLExpEqual").val('');
    $("#ctl00_ContentPlaceHolder1_txtLExpLessAndEqual").val('');
    $("#ctl00_ContentPlaceHolder1_txtLExpGreaterAndEqual").val('');

    $("#tblHours").css("display", "none");
    $("#ctl00_ContentPlaceHolder1_txtHExpEqual").val('');
    $("#ctl00_ContentPlaceHolder1_txtHExpLessAndEqual").val('');
    $("#ctl00_ContentPlaceHolder1_txtHExpGreaterAndEqual").val('');

    $("#tblMaterialExpense").css("display", "none");
    $("#ctl00_ContentPlaceHolder1_txtMExpEqual").val('');
    $("#ctl00_ContentPlaceHolder1_txtMExpLessAndEqual").val('');
    $("#ctl00_ContentPlaceHolder1_txtMExpGreaterAndEqual").val('');

    $("#tblTotalBilled").css("display", "none");
    $("#ctl00_ContentPlaceHolder1_txtBExpEqual").val('');
    $("#ctl00_ContentPlaceHolder1_txtBExpLessAndEqual").val('');
    $("#ctl00_ContentPlaceHolder1_txtBExpGreaterAndEqual").val('');

    $("#tblTotalExpenses").css("display", "none");
    $("#ctl00_ContentPlaceHolder1_txtTExpEqual").val('');
    $("#ctl00_ContentPlaceHolder1_txtTExpLessAndEqual").val('');
    $("#ctl00_ContentPlaceHolder1_txtTExpGreaterAndEqual").val('');

    $("#tblTotalOnOrder").css("display", "none");
    $("#ctl00_ContentPlaceHolder1_txtTOExpEqual").val('');
    $("#ctl00_ContentPlaceHolder1_txtTOExpLessAndEqual").val('');
    $("#ctl00_ContentPlaceHolder1_txtTOExpGreaterAndEqual").val('');
    //Expenses


    $("#tblStatus").css("display", "none");
    $("#ctl00_ContentPlaceHolder1_drpStatus").val('All');


    $("#tblCustomer").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpCustomer input[type="checkbox"]').attr('checked', false);

    $("#tblLocation").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpLocation input[type="checkbox"]').attr('checked', false);

    //Rahil
    $("#tblDescription").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpDescription input[type="checkbox"]').attr('checked', false);

    $("#tblAmountDue").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpAmountDue input[type="checkbox"]').attr('checked', false);

    $("#tblProject").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpProject input[type="checkbox"]').attr('checked', false);

    $("#tblDateCreated").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpDateCreated input[type="checkbox"]').attr('checked', false);

    $("#tblLocationRemarks").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpLocationRemarks input[type="checkbox"]').attr('checked', false);

    $("#tblTotal").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpTotal input[type="checkbox"]').attr('checked', false);

    $("#tblManualInvoice").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpManualInvoice input[type="checkbox"]').attr('checked', false);

    $("#tblCustomerName").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpCustomerName input[type="checkbox"]').attr('checked', false);

    $("#tblCustomerName").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpCustomerName input[type="checkbox"]').attr('checked', false);

    $("#tblType").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpType input[type="checkbox"]').attr('checked', false);

    $("#tblBatch").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpBatch input[type="checkbox"]').attr('checked', false);

    $("#tblBalance").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpBalance input[type="checkbox"]').attr('checked', false);

    $("#tblDueDate").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpDueDate input[type="checkbox"]').attr('checked', false);

    $("#tblInUse").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpInUse input[type="checkbox"]').attr('checked', false);

    $("#tblShipVia").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpShipVia input[type="checkbox"]').attr('checked', false);

    $("#tblQBVendorID").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpQBVendorID input[type="checkbox"]').attr('checked', false);

    $("#tblOnePer").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpOnePer input[type="checkbox"]').attr('checked', false);

    $("#tblDBank").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpDBank input[type="checkbox"]').attr('checked', false);

    $("#tblPO").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpPO input[type="checkbox"]').attr('checked', false);

    $("#tblTerms").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpTerms input[type="checkbox"]').attr('checked', false);

    $("#tblDays").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpDays input[type="checkbox"]').attr('checked', false);

    $("#tblAcc").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpAcc input[type="checkbox"]').attr('checked', false);

    $("#tblTotalBilled").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpTotalBilled input[type="checkbox"]').attr('checked', false);

    $("#tblSalesTax").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpSalesTax input[type="checkbox"]').attr('checked', false);

    $("#tblPreTaxAmount").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpPreTaxAmount input[type="checkbox"]').attr('checked', false);

    $("#tblRemit").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpRemit input[type="checkbox"]').attr('checked', false);


    //Rahil

    $("#tblHours").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpHours input[type="checkbox"]').attr('checked', false);

    $("#tblLocationAddress").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpLocationAddress input[type="checkbox"]').attr('checked', false);

    $("#tblLocationCity").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpLocationCity input[type="checkbox"]').attr('checked', false);

    $("#tblLocationState").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpLocationState input[type="checkbox"]').attr('checked', false);

    $("#tblLocationZip").css("display", "none");
    $("#ctl00_ContentPlaceHolder1_txtLocationZip").val('');

    $("#tblLocationType").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpLocationType input[type="checkbox"]').attr('checked', false);

    $("#tblEquipmentName").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpEquipmentName input[type="checkbox"]').attr('checked', false);

    $("#tblManuf").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpManuf input[type="checkbox"]').attr('checked', false);

    $("#tblEquipmentType").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpEquipmentType input[type="checkbox"]').attr('checked', false);

    $("#tblServiceType").css("display", "none");
    $('#ctl00_ContentPlaceHolder1_drpServiceType input[type="checkbox"]').attr('checked', false);

    $("#tblLoc").css("display", "none");
    $("#ctl00_ContentPlaceHolder1_txtLocEqual").val('');
    $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").val('');
    $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").val('');

    $("#tblEquipmentCounts").css("display", "none");
    $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsEqual").val('');
    $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsLessAndEqual").val('');
    $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsGreatAndEqual").val('');

    //Added by Yashasvi Jadav
    $("#tblEquip").css("display", "none");
    $("#ctl00_ContentPlaceHolder1_txtEquipEqual").val('');
    $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").val('');
    $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").val('');

    $("#tblOpenCalls").css("display", "none");
    $("#ctl00_ContentPlaceHolder1_txtOCEqual").val('');
    $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").val('');
    $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").val('');


    $('#tblBalance input[name="ctl00$ContentPlaceHolder1$Balance"][value="rdbAny"]').prop('checked', true);
    $("#ctl00_ContentPlaceHolder1_txtBalEqual").attr("disabled", "disabled");
    $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").attr("disabled", "disabled");
    $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").attr("disabled", "disabled");

    //Expenses
    $('#tblExpenses input[name="ctl00$ContentPlaceHolder1$Expenses"][value="rdbExpAny"]').prop('checked', true);
    $("#ctl00_ContentPlaceHolder1_txtExpEqual").attr("disabled", "disabled");
    $("#ctl00_ContentPlaceHolder1_txtExpLessAndEqual").attr("disabled", "disabled");
    $("#ctl00_ContentPlaceHolder1_txtExpGreaterAndEqual").attr("disabled", "disabled");

    $('#tblLaborExpense input[name="ctl00$ContentPlaceHolder1$LaborExpense"][value="rdbLExpAny"]').prop('checked', true);
    $("#ctl00_ContentPlaceHolder1_txtLExpEqual").attr("disabled", "disabled");
    $("#ctl00_ContentPlaceHolder1_txtLExpLessAndEqual").attr("disabled", "disabled");
    $("#ctl00_ContentPlaceHolder1_txtLExpGreaterAndEqual").attr("disabled", "disabled");

    $('#tblHours input[name="ctl00$ContentPlaceHolder1$Hours"][value="rdbHExpAny"]').prop('checked', true);
    $("#ctl00_ContentPlaceHolder1_txtHExpEqual").attr("disabled", "disabled");
    $("#ctl00_ContentPlaceHolder1_txtHExpLessAndEqual").attr("disabled", "disabled");
    $("#ctl00_ContentPlaceHolder1_txtHExpGreaterAndEqual").attr("disabled", "disabled");

    $('#tblMaterialExpense input[name="ctl00$ContentPlaceHolder1$MaterialExpense"][value="rdbMExpAny"]').prop('checked', true);
    $("#ctl00_ContentPlaceHolder1_txtMExpEqual").attr("disabled", "disabled");
    $("#ctl00_ContentPlaceHolder1_txtMExpLessAndEqual").attr("disabled", "disabled");
    $("#ctl00_ContentPlaceHolder1_txtMExpGreaterAndEqual").attr("disabled", "disabled");

    $('#tblTotalBilled input[name="ctl00$ContentPlaceHolder1$TotalBilled"][value="rdbBExpAny"]').prop('checked', true);
    $("#ctl00_ContentPlaceHolder1_txtBExpEqual").attr("disabled", "disabled");
    $("#ctl00_ContentPlaceHolder1_txtBExpLessAndEqual").attr("disabled", "disabled");
    $("#ctl00_ContentPlaceHolder1_txtBExpGreaterAndEqual").attr("disabled", "disabled");

    $('#tblTotalExpenses input[name="ctl00$ContentPlaceHolder1$TotalExpenses"][value="rdbTExpAny"]').prop('checked', true);
    $("#ctl00_ContentPlaceHolder1_txtTExpEqual").attr("disabled", "disabled");
    $("#ctl00_ContentPlaceHolder1_txtTExpLessAndEqual").attr("disabled", "disabled");
    $("#ctl00_ContentPlaceHolder1_txtTExpGreaterAndEqual").attr("disabled", "disabled");

    $('#tblTotalOnOrder input[name="ctl00$ContentPlaceHolder1$TotalOnOrder"][value="rdbTOExpAny"]').prop('checked', true);
    $("#ctl00_ContentPlaceHolder1_txtTOExpEqual").attr("disabled", "disabled");
    $("#ctl00_ContentPlaceHolder1_txtTOExpLessAndEqual").attr("disabled", "disabled");
    $("#ctl00_ContentPlaceHolder1_txtTOExpGreaterAndEqual").attr("disabled", "disabled");
    //Expenses

    $('#tblLoc input[name="ctl00$ContentPlaceHolder1$loc"][value="rdbLocAny"]').prop('checked', true);
    $("#ctl00_ContentPlaceHolder1_txtLocEqual").attr("disabled", "disabled");
    $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").attr("disabled", "disabled");
    $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").attr("disabled", "disabled");

    $('#tblEquip input[name="ctl00$ContentPlaceHolder1$equip"][value="rdbEquipAny"]').prop('checked', true);
    $("#ctl00_ContentPlaceHolder1_txtEquipEqual").attr("disabled", "disabled");
    $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").attr("disabled", "disabled");
    $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").attr("disabled", "disabled");

    //Added by Yashasvi Jadav
    $('#tblEquipmentCounts input[name="ctl00$ContentPlaceHolder1$equipmentcounts"][value="rdbEquipmentCountsAny"]').prop('checked', true);
    $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsEqual").attr("disabled", "disabled");
    $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsLessAndEqual").attr("disabled", "disabled");
    $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsGreatAndEqual").attr("disabled", "disabled");

    $('#tblOpenCalls input[name="ctl00$ContentPlaceHolder1$oc"][value="rdbOCAny"]').prop('checked', true);
    $("#ctl00_ContentPlaceHolder1_txtOCEqual").attr("disabled", "disabled");
    $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").attr("disabled", "disabled");
    $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").attr("disabled", "disabled");

    $('#tblEquipmentPrice input[name="ctl00$ContentPlaceHolder1$ep"][value="rdbEquipmentPriceAny"]').prop('checked', true);
    $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").attr("disabled", "disabled");
    $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").attr("disabled", "disabled");
    $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").attr("disabled", "disabled");
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
    //        if ($("#ctl00_ContentPlaceHolder1_txtBalEqual").val() == '' && $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").val() == '' && $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").val() == '') {
    //            $('#tblBalance input[name="ctl00$ContentPlaceHolder1$Balance"][value="rdbAny"]').prop('checked', true);
    //            $("#ctl00_ContentPlaceHolder1_txtBalEqual").attr("disabled", "disabled");
    //            $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").attr("disabled", "disabled");
    //            $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").attr("disabled", "disabled");
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

    if (filterName == "Project#") {
        filterName = "Project";
    }


    if (filterName != "loc" || filterName != "equip" || filterName != "opencall") {
        filterName = filterName.replace(" ", "");
        $("#tbl" + filterName).fadeIn(200).css("display", "block");
    }

    if (filterName == "Balance") {
        $("#tblBalance").fadeIn(200).css("display", "block");
        if ($("#ctl00_ContentPlaceHolder1_txtBalEqual").val() == '' && $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").val() == '' && $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").val() == '') {
            $('#tblBalance input[name="ctl00$ContentPlaceHolder1$Balance"][value="rdbAny"]').prop('checked', true);
            $("#ctl00_ContentPlaceHolder1_txtBalEqual").attr("disabled", "disabled");
            $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").attr("disabled", "disabled");
            $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").attr("disabled", "disabled");
        }

    }
    //Expenses
    if (filterName == "Expenses") {
        $("#tblExpenses").fadeIn(200).css("display", "block");
        if ($("#ctl00_ContentPlaceHolder1_txtExpEqual").val() == '' && $("#ctl00_ContentPlaceHolder1_txtExpLessAndEqual").val() == '' && $("#ctl00_ContentPlaceHolder1_txtExpGreaterAndEqual").val() == '') {
            $('#tblExpenses input[name="ctl00$ContentPlaceHolder1$Expenses"][value="rdbExpAny"]').prop('checked', true);
            $("#ctl00_ContentPlaceHolder1_txtExpEqual").attr("disabled", "disabled");
            $("#ctl00_ContentPlaceHolder1_txtExpLessAndEqual").attr("disabled", "disabled");
            $("#ctl00_ContentPlaceHolder1_txtExpGreaterAndEqual").attr("disabled", "disabled");
        }

    }

    if (filterName == "LaborExpense") {
        $("#tblLaborExpense").fadeIn(200).css("display", "block");
        if ($("#ctl00_ContentPlaceHolder1_txtLExpEqual").val() == '' && $("#ctl00_ContentPlaceHolder1_txtLExpLessAndEqual").val() == '' && $("#ctl00_ContentPlaceHolder1_txtLExpGreaterAndEqual").val() == '') {
            $('#tblLaborExpense input[name="ctl00$ContentPlaceHolder1$LaborExpense"][value="rdbLExpAny"]').prop('checked', true);
            $("#ctl00_ContentPlaceHolder1_txtLExpEqual").attr("disabled", "disabled");
            $("#ctl00_ContentPlaceHolder1_txtLExpLessAndEqual").attr("disabled", "disabled");
            $("#ctl00_ContentPlaceHolder1_txtLExpGreaterAndEqual").attr("disabled", "disabled");
        }

    }

    if (filterName == "MaterialExpense") {
        $("#tblMaterialExpense").fadeIn(200).css("display", "block");
        if ($("#ctl00_ContentPlaceHolder1_txtMExpEqual").val() == '' && $("#ctl00_ContentPlaceHolder1_txtMExpLessAndEqual").val() == '' && $("#ctl00_ContentPlaceHolder1_txtMExpGreaterAndEqual").val() == '') {
            $('#tblMaterialExpense input[name="ctl00$ContentPlaceHolder1$MaterialExpense"][value="rdbMExpAny"]').prop('checked', true);
            $("#ctl00_ContentPlaceHolder1_txtMExpEqual").attr("disabled", "disabled");
            $("#ctl00_ContentPlaceHolder1_txtMExpLessAndEqual").attr("disabled", "disabled");
            $("#ctl00_ContentPlaceHolder1_txtMExpGreaterAndEqual").attr("disabled", "disabled");
        }

    }

    if (filterName == "TotalBilled") {
        $("#tblTotalBilled").fadeIn(200).css("display", "block");
        if ($("#ctl00_ContentPlaceHolder1_txtBExpEqual").val() == '' && $("#ctl00_ContentPlaceHolder1_txtBExpLessAndEqual").val() == '' && $("#ctl00_ContentPlaceHolder1_txtBExpGreaterAndEqual").val() == '') {
            $('#tblTotalBilled input[name="ctl00$ContentPlaceHolder1$MaterialExpense"][value="rdbMExpAny"]').prop('checked', true);
            $("#ctl00_ContentPlaceHolder1_txtBExpEqual").attr("disabled", "disabled");
            $("#ctl00_ContentPlaceHolder1_txtBExpLessAndEqual").attr("disabled", "disabled");
            $("#ctl00_ContentPlaceHolder1_txtBExpGreaterAndEqual").attr("disabled", "disabled");
        }

    }


    if (filterName == "Hours") {
        $("#tblHours").fadeIn(200).css("display", "block");
        if ($("#ctl00_ContentPlaceHolder1_txtHExpEqual").val() == '' && $("#ctl00_ContentPlaceHolder1_txtHExpLessAndEqual").val() == '' && $("#ctl00_ContentPlaceHolder1_txtHExpGreaterAndEqual").val() == '') {
            $('#tblHours input[name="ctl00$ContentPlaceHolder1$Hours"][value="rdbHExpAny"]').prop('checked', true);
            $("#ctl00_ContentPlaceHolder1_txtHExpEqual").attr("disabled", "disabled");
            $("#ctl00_ContentPlaceHolder1_txtHExpLessAndEqual").attr("disabled", "disabled");
            $("#ctl00_ContentPlaceHolder1_txtHExpGreaterAndEqual").attr("disabled", "disabled");
        }

    }

    if (filterName == "TotalExpenses") {
        $("#tblTotalExpenses").fadeIn(200).css("display", "block");
        if ($("#ctl00_ContentPlaceHolder1_txtTExpEqual").val() == '' && $("#ctl00_ContentPlaceHolder1_txtTExpLessAndEqual").val() == '' && $("#ctl00_ContentPlaceHolder1_txtTExpGreaterAndEqual").val() == '') {
            $('#tblTotalExpenses input[name="ctl00$ContentPlaceHolder1$TotalExpenses"][value="rdbTExpAny"]').prop('checked', true);
            $("#ctl00_ContentPlaceHolder1_txtTExpEqual").attr("disabled", "disabled");
            $("#ctl00_ContentPlaceHolder1_txtTExpLessAndEqual").attr("disabled", "disabled");
            $("#ctl00_ContentPlaceHolder1_txtTExpGreaterAndEqual").attr("disabled", "disabled");
        }

    }

    if (filterName == "TotalOnOrder") {
        $("#tblTotalOnOrder").fadeIn(200).css("display", "block");
        if ($("#ctl00_ContentPlaceHolder1_txtTOExpEqual").val() == '' && $("#ctl00_ContentPlaceHolder1_txtTOExpLessAndEqual").val() == '' && $("#ctl00_ContentPlaceHolder1_txtTOExpGreaterAndEqual").val() == '') {
            $('#tblTotalOnOrder input[name="ctl00$ContentPlaceHolder1$TotalExpenses"][value="rdbTOExpAny"]').prop('checked', true);
            $("#ctl00_ContentPlaceHolder1_txtTOExpEqual").attr("disabled", "disabled");
            $("#ctl00_ContentPlaceHolder1_txtTOExpLessAndEqual").attr("disabled", "disabled");
            $("#ctl00_ContentPlaceHolder1_txtTOExpGreaterAndEqual").attr("disabled", "disabled");
        }

    }
    //Expenses


    //    else {
    //        $("#tblBalance").css("display", "none");
    //    }

    if (filterName == "loc") {
        $("#tblLoc").fadeIn(200).css("display", "block");
        if ($("#ctl00_ContentPlaceHolder1_txtLocEqual").val() == '' && $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").val() == '' && $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").val() == '') {
            $('#tblLoc input[name="ctl00$ContentPlaceHolder1$loc"][value="rdbLocAny"]').prop('checked', true);
            $("#ctl00_ContentPlaceHolder1_txtLocEqual").attr("disabled", "disabled");
            $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").attr("disabled", "disabled");
            $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").attr("disabled", "disabled");
        }

    }
    //    else {

    //        $("#tblLoc").css("display", "none");
    //    }

    if (filterName == "equip") {
        $("#tblEquip").fadeIn(200).css("display", "block");
        if ($("#ctl00_ContentPlaceHolder1_txtEquipEqual").val() == '' && $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").val() == '' && $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").val() == '') {
            $('#tblEquip input[name="ctl00$ContentPlaceHolder1$equip"][value="rdbEquipAny"]').prop('checked', true);
            $("#ctl00_ContentPlaceHolder1_txtEquipEqual").attr("disabled", "disabled");
            $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").attr("disabled", "disabled");
            $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").attr("disabled", "disabled");
        }

    }

    if (filterName == "EquipmentCounts") {
        $("#tblEquipmentCounts").fadeIn(200).css("display", "block");
        if ($("#ctl00_ContentPlaceHolder1_txtEquipmentCountsEqual").val() == '' && $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsLessAndEqual").val() == '' && $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsGreatAndEqual").val() == '') {
            $('#tblEquipmentCounts input[name="ctl00$ContentPlaceHolder1$equipmentcounts"][value="rdbEquipmentCountsAny"]').prop('checked', true);
            $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsEqual").attr("disabled", "disabled");
            $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsLessAndEqual").attr("disabled", "disabled");
            $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsGreatAndEqual").attr("disabled", "disabled");
        }

    }

    //    else {

    //        $("#tblEquip").css("display", "none");
    //    }

    if (filterName == "opencall") {
        $("#tblOpenCalls").fadeIn(200).css("display", "block");
        if ($("#ctl00_ContentPlaceHolder1_txtOCEqual").val() == '' && $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").val() == '' && $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").val() == '') {
            $('#tblOpenCalls input[name="ctl00$ContentPlaceHolder1$oc"][value="rdbOCAny"]').prop('checked', true);
            $("#ctl00_ContentPlaceHolder1_txtOCEqual").attr("disabled", "disabled");
            $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").attr("disabled", "disabled");
            $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").attr("disabled", "disabled");
        }

    }
    //    else {

    //        $("#tblOpenCalls").css("display", "none");
    //    }

    if (filterName == "EquipmentPrice") {
        $("#tblEquipmentPrice").fadeIn(200).css("display", "block");
        if ($("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").val() == '' && $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").val() == '' && $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").val() == '') {
            $('#tblEquipmentPrice input[name="ctl00$ContentPlaceHolder1$ep"][value="rdbEquipmentPriceAny"]').prop('checked', true);
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").attr("disabled", "disabled");
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").attr("disabled", "disabled");
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").attr("disabled", "disabled");
        }

    }
}

function setFiltersValue(filterName, filterValue) {
    if (filterName == "Name") {
        //$("#ctl00_ContentPlaceHolder1_txtName").val(filterValue);
        //                filterValue == "" ? ($("#ctl00_ContentPlaceHolder1_drpName").val('All')) : ($("#ctl00_ContentPlaceHolder1_drpName option:contains('" + filterValue + "')").prop('selected', true));
        // filterValue == "" ? ($("#ctl00_ContentPlaceHolder1_drpName").val('All')) : ($("#ctl00_ContentPlaceHolder1_drpName").val(filterValue));
        $('#ctl00_ContentPlaceHolder1_drpName input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#ctl00_ContentPlaceHolder1_drpName input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#ctl00_ContentPlaceHolder1_drpName input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "Equipment") {
        //$("#ctl00_ContentPlaceHolder1_txtName").val(filterValue);
        //                filterValue == "" ? ($("#ctl00_ContentPlaceHolder1_drpName").val('All')) : ($("#ctl00_ContentPlaceHolder1_drpName option:contains('" + filterValue + "')").prop('selected', true));
        // filterValue == "" ? ($("#ctl00_ContentPlaceHolder1_drpName").val('All')) : ($("#ctl00_ContentPlaceHolder1_drpName").val(filterValue));
        $('#ctl00_ContentPlaceHolder1_drpEquipmentState input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#ctl00_ContentPlaceHolder1_drpEquipmentState input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#ctl00_ContentPlaceHolder1_drpEquipmentState input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "City") {
        //                $("#ctl00_ContentPlaceHolder1_txtCity").val(filterValue);
        $('#ctl00_ContentPlaceHolder1_drpCity input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedCity = filterValue.split("|");
            for (i = 0; i <= splitedCity.length - 1; i++) {
                $('#ctl00_ContentPlaceHolder1_drpCity input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedCity[i] = splitedCity[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedCity[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#ctl00_ContentPlaceHolder1_drpCity input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "State") {
        //                filterValue == "" ? ($("#ctl00_ContentPlaceHolder1_ddlState").val('All')) : ($("#ctl00_ContentPlaceHolder1_ddlState option:contains('" + filterValue + "')").prop('selected', true));
        //  filterValue == "" ? ($("#ctl00_ContentPlaceHolder1_ddlState").val('All')) : ($("#ctl00_ContentPlaceHolder1_ddlState").val(filterValue));
        $('#ctl00_ContentPlaceHolder1_ddlState input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#ctl00_ContentPlaceHolder1_ddlState input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    actualText = splitedNames[i].replace("'", "").replace("'", "");
                    // alert(actualText);
                    // alert(actualText.trim().length);
                    if (actualText.trim().length <= 2) {

                        $("#ctl00_ContentPlaceHolder1_ddlStateReference option[value=" + actualText.trim() + "]").attr("selected", "selected");
                    }
                    else {
                        $("#ctl00_ContentPlaceHolder1_ddlStateReference option:contains('" + actualText.trim() + "')").attr("selected", "selected")
                    }

                    var getText = $("#ctl00_ContentPlaceHolder1_ddlStateReference option:selected").text();
                    // alert(getText);
                    if (lblValue.trim() == getText.trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#ctl00_ContentPlaceHolder1_ddlState input[type="checkbox"]').attr('checked', false);
        }

    }

    if (filterName == "LocationState") {
        //                filterValue == "" ? ($("#ctl00_ContentPlaceHolder1_ddlState").val('All')) : ($("#ctl00_ContentPlaceHolder1_ddlState option:contains('" + filterValue + "')").prop('selected', true));
        //  filterValue == "" ? ($("#ctl00_ContentPlaceHolder1_ddlState").val('All')) : ($("#ctl00_ContentPlaceHolder1_ddlState").val(filterValue));
        $('#ctl00_ContentPlaceHolder1_drpLocationState input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedLocStates = filterValue.split("|");
            for (i = 0; i <= splitedLocStates.length - 1; i++) {
                $('#ctl00_ContentPlaceHolder1_drpLocationState input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    actualText = splitedLocStates[i].replace("'", "").replace("'", "");
                    // alert(actualText);
                    // alert(actualText.trim().length);
                    if (actualText.trim().length <= 2) {

                        $("#ctl00_ContentPlaceHolder1_ddlStateReference option[value=" + actualText.trim() + "]").attr("selected", "selected");
                    }
                    else {
                        $("#ctl00_ContentPlaceHolder1_ddlStateReference option:contains('" + actualText.trim() + "')").attr("selected", "selected")
                    }

                    var getText = $("#ctl00_ContentPlaceHolder1_ddlStateReference option:selected").text();
                    // alert(getText);
                    if (lblValue.trim() == getText.trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#ctl00_ContentPlaceHolder1_drpLocationState input[type="checkbox"]').attr('checked', false);
        }

    }

    if (filterName == "Zip") {
        $("#ctl00_ContentPlaceHolder1_txtZip").val(filterValue);
    }

    if (filterName == "InstalledOn") {
        $("#ctl00_ContentPlaceHolder1_InstalledOn").val(filterValue);
    }

    if (filterName == "Phone") {
        $("#ctl00_ContentPlaceHolder1_txtPhone").val(filterValue);
    }

    if (filterName == "Fax") {
        $("#ctl00_ContentPlaceHolder1_txtFax").val(filterValue);
    }

    if (filterName == "Contact") {
        $("#ctl00_ContentPlaceHolder1_txtContact").val(filterValue);
    }

    if (filterName == "Address") {
        //                $("#ctl00_ContentPlaceHolder1_txtAddress").val(filterValue);
        $('#ctl00_ContentPlaceHolder1_drpAddress input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedAddress = filterValue.split("|");
            for (i = 0; i <= splitedAddress.length - 1; i++) {
                $('#ctl00_ContentPlaceHolder1_drpAddress input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedAddress[i] = splitedAddress[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedAddress[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#ctl00_ContentPlaceHolder1_drpAddress input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "Email") {
        $("#ctl00_ContentPlaceHolder1_txtEmail").val(filterValue);
    }

    if (filterName == "Country") {
        $("#ctl00_ContentPlaceHolder1_txtCountry").val(filterValue);
    }

    if (filterName == "Website") {
        $("#ctl00_ContentPlaceHolder1_txtWebsite").val(filterValue);
    }

    if (filterName == "Cellular") {
        $("#ctl00_ContentPlaceHolder1_txtCellular").val(filterValue);
    }

    if (filterName == "Category") {
        filterValue == "" ? ($("#ctl00_ContentPlaceHolder1_drpCategory").val('All')) : ($("#ctl00_ContentPlaceHolder1_drpCategory option:selected").val(filterValue));
    }

    /*
    if (filterName == "Type") {
        // filterValue == "" ? ($("#ctl00_ContentPlaceHolder1_drpType").val('All')) : ($("#ctl00_ContentPlaceHolder1_drpType option:contains('" + filterValue + "')").prop('selected', true));
        $('#ctl00_ContentPlaceHolder1_drpType input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedTypes = filterValue.split("|");
            for (i = 0; i <= splitedTypes.length - 1; i++) {
                $('#ctl00_ContentPlaceHolder1_drpType input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedTypes[i] = splitedTypes[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedTypes[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#ctl00_ContentPlaceHolder1_drpType input[type="checkbox"]').attr('checked', false);
        }
    }*/

    if (filterName == "Balance") {
        if (filterValue == "") {
            $('#tblBalance input[name="ctl00$ContentPlaceHolder1$Balance"][value="rdbAny"]').prop('checked', true);
            $("#ctl00_ContentPlaceHolder1_txtBalEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtBalEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").attr("disabled", true);
        }
        else if ((filterValue.indexOf(">=") != -1 || filterValue.indexOf("&gt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").attr("disabled", false);
            $('#tblBalance input[name="ctl00$ContentPlaceHolder1$Balance"][value="rdbGreaterAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtBalEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtBalEqual").attr("disabled", 'disabled');
            $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").attr("disabled", 'disabled');
        }
        else if ((filterValue.indexOf("<=") != -1 || filterValue.indexOf("&lt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").attr("disabled", false);
            $('#tblBalance input[name="ctl00$ContentPlaceHolder1$Balance"][value="rdbLessAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtBalEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtBalEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").attr("disabled", true);
        }
        else if (filterValue.indexOf('and') != -1) {
            var splitedBalance = '';
            var greaterValue = '';
            var lessValue = '';
            splitedBalance = filterValue.split('and');
            greaterValue = splitedBalance[0].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            lessValue = splitedBalance[1].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").val(greaterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").val(lessValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtBalEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtBalEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").attr("disabled", false);
            $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").attr("disabled", false);
            $('#tblBalance input[name="ctl00$ContentPlaceHolder1$Balance"][value="rdbLessAndEqual"]').prop('checked', true);
        }
        else {
            $('#tblBalance input[name="ctl00$ContentPlaceHolder1$Balance"][value="rdbEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtBalEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").attr("disabled", true);

        }

        //                filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
        //                $("#ctl00_ContentPlaceHolder1_txtBalance").val(filterValue.trim());
    }


    //Expenses
    if (filterName == "Expenses") {
        if (filterValue == "") {
            $('#tblExpenses input[name="ctl00$ContentPlaceHolder1$Expenses"][value="rdbExpAny"]').prop('checked', true);
            $("#ctl00_ContentPlaceHolder1_txtExpEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtExpEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtExpGreaterAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtExpGreaterAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtExpLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtExpLessAndEqual").attr("disabled", true);
        }
        else if ((filterValue.indexOf(">=") != -1 || filterValue.indexOf("&gt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#ctl00_ContentPlaceHolder1_txtExpGreaterAndEqual").attr("disabled", false);
            $('#tblExpenses input[name="ctl00$ContentPlaceHolder1$Expenses"][value="rdbExpGreaterAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtExpGreaterAndEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtExpEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtExpEqual").attr("disabled", 'disabled');
            $("#ctl00_ContentPlaceHolder1_txtExpLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtExpLessAndEqual").attr("disabled", 'disabled');
        }
        else if ((filterValue.indexOf("<=") != -1 || filterValue.indexOf("&lt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#ctl00_ContentPlaceHolder1_txtExpLessAndEqual").attr("disabled", false);
            $('#tblExpenses input[name="ctl00$ContentPlaceHolder1$Expenses"][value="rdbExpLessAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtExpLessAndEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtExpEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtExpEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtExpGreaterAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtExpGreaterAndEqual").attr("disabled", true);
        }
        else if (filterValue.indexOf('and') != -1) {
            var splitedExpenses = '';
            var greaterValue = '';
            var lessValue = '';
            splitedExpenses = filterValue.split('and');
            greaterValue = splitedExpenses[0].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            lessValue = splitedExpenses[1].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtExpGreaterAndEqual").val(greaterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtExpLessAndEqual").val(lessValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtExpEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtExpEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtExpGreaterAndEqual").attr("disabled", false);
            $("#ctl00_ContentPlaceHolder1_txtExpLessAndEqual").attr("disabled", false);
            $('#tblExpenses input[name="ctl00$ContentPlaceHolder1$Expenses"][value="rdbExpLessAndEqual"]').prop('checked', true);
        }
        else {
            $('#tblExpenses input[name="ctl00$ContentPlaceHolder1$Expenses"][value="rdbExpEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtExpEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtExpGreaterAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtExpGreaterAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtExpLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtExpLessAndEqual").attr("disabled", true);

        }

        //                filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
        //                $("#ctl00_ContentPlaceHolder1_txtBalance").val(filterValue.trim());
    }
    
    if (filterName == "LaborExpense") {
        if (filterValue == "") {
            $('#tblLaborExpense input[name="ctl00$ContentPlaceHolder1$LaborExpense"][value="rdbLExpAny"]').prop('checked', true);
            $("#ctl00_ContentPlaceHolder1_txtLExpEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtLExpEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtLExpGreaterAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtLExpGreaterAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtLExpLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtLExpLessAndEqual").attr("disabled", true);
        }
        else if ((filterValue.indexOf(">=") != -1 || filterValue.indexOf("&gt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#ctl00_ContentPlaceHolder1_txtLExpGreaterAndEqual").attr("disabled", false);
            $('#tblLaborExpense input[name="ctl00$ContentPlaceHolder1$LaborExpense"][value="rdbLExpGreaterAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtLExpGreaterAndEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtLExpEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtLExpEqual").attr("disabled", 'disabled');
            $("#ctl00_ContentPlaceHolder1_txtLExpLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtLExpLessAndEqual").attr("disabled", 'disabled');
        }
        else if ((filterValue.indexOf("<=") != -1 || filterValue.indexOf("&lt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#ctl00_ContentPlaceHolder1_txtLExpLessAndEqual").attr("disabled", false);
            $('#tblLaborExpense input[name="ctl00$ContentPlaceHolder1$LaborExpense"][value="rdbLExpLessAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtLExpLessAndEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtLExpEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtLExpEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtLExpGreaterAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtLExpGreaterAndEqual").attr("disabled", true);
        }
        else if (filterValue.indexOf('and') != -1) {
            var splitedExpenses = '';
            var greaterValue = '';
            var lessValue = '';
            splitedExpenses = filterValue.split('and');
            greaterValue = splitedExpenses[0].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            lessValue = splitedExpenses[1].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtLExpGreaterAndEqual").val(greaterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtLExpLessAndEqual").val(lessValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtLExpEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtLExpEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtLExpGreaterAndEqual").attr("disabled", false);
            $("#ctl00_ContentPlaceHolder1_txtLExpLessAndEqual").attr("disabled", false);
            $('#tblLaborExpenses input[name="ctl00$ContentPlaceHolder1$LaborExpense"][value="rdbLExpLessAndEqual"]').prop('checked', true);
        }
        else {
            $('#tblLaborExpense input[name="ctl00$ContentPlaceHolder1$LaborExpense"][value="rdbLExpEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtLExpEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtLExpGreaterAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtLExpGreaterAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtLExpLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtLExpLessAndEqual").attr("disabled", true);

        }

        //                filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
        //                $("#ctl00_ContentPlaceHolder1_txtBalance").val(filterValue.trim());
    }

    if (filterName == "MaterialExpense") {
        if (filterValue == "") {
            $('#tblMaterialExpense input[name="ctl00$ContentPlaceHolder1$MaterialExpense"][value="rdbMExpAny"]').prop('checked', true);
            $("#ctl00_ContentPlaceHolder1_txtMExpEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtMExpEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtMExpGreaterAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtMExpGreaterAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtMExpLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtMExpLessAndEqual").attr("disabled", true);
        }
        else if ((filterValue.indexOf(">=") != -1 || filterValue.indexOf("&gt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#ctl00_ContentPlaceHolder1_txtMExpGreaterAndEqual").attr("disabled", false);
            $('#tblMaterialExpense input[name="ctl00$ContentPlaceHolder1$MaterialExpense"][value="rdbMExpGreaterAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtMExpGreaterAndEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtMExpEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtMExpEqual").attr("disabled", 'disabled');
            $("#ctl00_ContentPlaceHolder1_txtMExpLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtMExpLessAndEqual").attr("disabled", 'disabled');
        }
        else if ((filterValue.indexOf("<=") != -1 || filterValue.indexOf("&lt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#ctl00_ContentPlaceHolder1_txtMExpLessAndEqual").attr("disabled", false);
            $('#tblMaterialExpense input[name="ctl00$ContentPlaceHolder1$MaterialExpense"][value="rdbMExpLessAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtMExpLessAndEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtMExpEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtMExpEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtMExpGreaterAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtMExpGreaterAndEqual").attr("disabled", true);
        }
        else if (filterValue.indexOf('and') != -1) {
            var splitedExpenses = '';
            var greaterValue = '';
            var lessValue = '';
            splitedExpenses = filterValue.split('and');
            greaterValue = splitedExpenses[0].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            lessValue = splitedExpenses[1].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtMExpGreaterAndEqual").val(greaterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtMExpLessAndEqual").val(lessValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtMExpEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtMExpEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtMExpGreaterAndEqual").attr("disabled", false);
            $("#ctl00_ContentPlaceHolder1_txtMExpLessAndEqual").attr("disabled", false);
            $('#tblMaterialExpense input[name="ctl00$ContentPlaceHolder1$MaterialExpense"][value="rdbMExpLessAndEqual"]').prop('checked', true);
        }
        else {
            $('#tblMaterialExpense input[name="ctl00$ContentPlaceHolder1$MaterialExpense"][value="rdbMExpEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtMExpEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtMExpGreaterAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtMExpGreaterAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtMExpLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtMExpLessAndEqual").attr("disabled", true);

        }

        //                filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
        //                $("#ctl00_ContentPlaceHolder1_txtBalance").val(filterValue.trim());
    }

    if (filterName == "TotalExpenses") {
        if (filterValue == "") {
            $('#tblTotalExpenses input[name="ctl00$ContentPlaceHolder1$TotalExpenses"][value="rdbTExpAny"]').prop('checked', true);
            $("#ctl00_ContentPlaceHolder1_txtTExpEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtTExpEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtTExpGreaterAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtTExpGreaterAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtTExpLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtTExpLessAndEqual").attr("disabled", true);
        }
        else if ((filterValue.indexOf(">=") != -1 || filterValue.indexOf("&gt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#ctl00_ContentPlaceHolder1_txtTExpGreaterAndEqual").attr("disabled", false);
            $('#tblTotalExpenses input[name="ctl00$ContentPlaceHolder1$TotalExpenses"][value="rdbTExpGreaterAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtTExpGreaterAndEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtTExpEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtTExpEqual").attr("disabled", 'disabled');
            $("#ctl00_ContentPlaceHolder1_txtTExpLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtTExpLessAndEqual").attr("disabled", 'disabled');
        }
        else if ((filterValue.indexOf("<=") != -1 || filterValue.indexOf("&lt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#ctl00_ContentPlaceHolder1_txtTExpLessAndEqual").attr("disabled", false);
            $('#tblTotalExpenses input[name="ctl00$ContentPlaceHolder1$TotalExpenses"][value="rdbTExpLessAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtTExpLessAndEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtTExpEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtTExpEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtTExpGreaterAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtTExpGreaterAndEqual").attr("disabled", true);
        }
        else if (filterValue.indexOf('and') != -1) {
            var splitedExpenses = '';
            var greaterValue = '';
            var lessValue = '';
            splitedExpenses = filterValue.split('and');
            greaterValue = splitedExpenses[0].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            lessValue = splitedExpenses[1].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtTExpGreaterAndEqual").val(greaterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtTExpLessAndEqual").val(lessValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtTExpEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtTExpEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtTExpGreaterAndEqual").attr("disabled", false);
            $("#ctl00_ContentPlaceHolder1_txtTExpLessAndEqual").attr("disabled", false);
            $('#tblTotalExpenses input[name="ctl00$ContentPlaceHolder1$TotalExpenses"][value="rdbTExpLessAndEqual"]').prop('checked', true);
        }
        else {
            $('#tblTotalExpenses input[name="ctl00$ContentPlaceHolder1$TotalExpenses"][value="rdbTExpEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtTExpEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtTExpGreaterAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtTExpGreaterAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtTExpLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtTExpLessAndEqual").attr("disabled", true);

        }

        //                filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
        //                $("#ctl00_ContentPlaceHolder1_txtBalance").val(filterValue.trim());
    }

    if (filterName == "TotalOnOrder") {
        if (filterValue == "") {
            $('#tblTotalOnOrder input[name="ctl00$ContentPlaceHolder1$TotalOnOrder"][value="rdbTOExpAny"]').prop('checked', true);
            $("#ctl00_ContentPlaceHolder1_txtTOExpEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtTOExpEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtTOExpGreaterAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtTOExpGreaterAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtTOExpLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtTOExpLessAndEqual").attr("disabled", true);
        }
        else if ((filterValue.indexOf(">=") != -1 || filterValue.indexOf("&gt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#ctl00_ContentPlaceHolder1_txtTOExpGreaterAndEqual").attr("disabled", false);
            $('#tblTotalOnOrder input[name="ctl00$ContentPlaceHolder1$TotalOnOrder"][value="rdbTOExpGreaterAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtTOExpGreaterAndEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtTOExpEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtTOExpEqual").attr("disabled", 'disabled');
            $("#ctl00_ContentPlaceHolder1_txtTOExpLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtTOExpLessAndEqual").attr("disabled", 'disabled');
        }
        else if ((filterValue.indexOf("<=") != -1 || filterValue.indexOf("&lt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#ctl00_ContentPlaceHolder1_txtTOExpLessAndEqual").attr("disabled", false);
            $('#tblTotalOnOrder input[name="ctl00$ContentPlaceHolder1$TotalOnOrder"][value="rdbTOExpLessAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtTOExpLessAndEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtTOExpEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtTOExpEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtTOExpGreaterAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtTOExpGreaterAndEqual").attr("disabled", true);
        }
        else if (filterValue.indexOf('and') != -1) {
            var splitedExpenses = '';
            var greaterValue = '';
            var lessValue = '';
            splitedExpenses = filterValue.split('and');
            greaterValue = splitedExpenses[0].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            lessValue = splitedExpenses[1].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtTOExpGreaterAndEqual").val(greaterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtTOExpLessAndEqual").val(lessValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtTOExpEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtTOExpEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtTOExpGreaterAndEqual").attr("disabled", false);
            $("#ctl00_ContentPlaceHolder1_txtTOExpLessAndEqual").attr("disabled", false);
            $('#tblTotalOnOrder input[name="ctl00$ContentPlaceHolder1$TotalOnOrder"][value="rdbTOExpLessAndEqual"]').prop('checked', true);
        }
        else {
            $('#tblTotalOnOrder input[name="ctl00$ContentPlaceHolder1$TotalOnOrder"][value="rdbTOExpEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtTOExpEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtTOExpGreaterAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtTOExpGreaterAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtTOExpLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtTOExpLessAndEqual").attr("disabled", true);

        }

        //                filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
        //                $("#ctl00_ContentPlaceHolder1_txtBalance").val(filterValue.trim());
    }

    if (filterName == "TotalBilled") {
        if (filterValue == "") {
            $('#tblTotalBilled input[name="ctl00$ContentPlaceHolder1$TotalBilled"][value="rdbBExpAny"]').prop('checked', true);
            $("#ctl00_ContentPlaceHolder1_txtBExpEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtBExpEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtBExpGreaterAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtBExpGreaterAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtBExpLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtBExpLessAndEqual").attr("disabled", true);
        }
        else if ((filterValue.indexOf(">=") != -1 || filterValue.indexOf("&gt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#ctl00_ContentPlaceHolder1_txtBExpGreaterAndEqual").attr("disabled", false);
            $('#tblTotalBilled input[name="ctl00$ContentPlaceHolder1$TotalBilled"][value="rdbBExpGreaterAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtBExpGreaterAndEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtBExpEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtBExpEqual").attr("disabled", 'disabled');
            $("#ctl00_ContentPlaceHolder1_txtBExpLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtBExpLessAndEqual").attr("disabled", 'disabled');
        }
        else if ((filterValue.indexOf("<=") != -1 || filterValue.indexOf("&lt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#ctl00_ContentPlaceHolder1_txtBExpLessAndEqual").attr("disabled", false);
            $('#tblTotalBilled input[name="ctl00$ContentPlaceHolder1$TotalBilled"][value="rdbBExpLessAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtBExpLessAndEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtBExpEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtBExpEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtBExpGreaterAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtBExpGreaterAndEqual").attr("disabled", true);
        }
        else if (filterValue.indexOf('and') != -1) {
            var splitedExpenses = '';
            var greaterValue = '';
            var lessValue = '';
            splitedExpenses = filterValue.split('and');
            greaterValue = splitedExpenses[0].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            lessValue = splitedExpenses[1].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtBExpGreaterAndEqual").val(greaterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtBExpLessAndEqual").val(lessValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtBExpEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtBExpEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtBExpGreaterAndEqual").attr("disabled", false);
            $("#ctl00_ContentPlaceHolder1_txtBExpLessAndEqual").attr("disabled", false);
            $('#tblTotalBilled input[name="ctl00$ContentPlaceHolder1$TotalBilled"][value="rdbBExpLessAndEqual"]').prop('checked', true);
        }
        else {
            $('#tblTotalBilled input[name="ctl00$ContentPlaceHolder1$TotalBilled"][value="rdbBExpEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtBExpEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtBExpGreaterAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtBExpGreaterAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtBExpLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtBExpLessAndEqual").attr("disabled", true);

        }

        //                filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
        //                $("#ctl00_ContentPlaceHolder1_txtBalance").val(filterValue.trim());
    }

    if (filterName == "Hours") {
        if (filterValue == "") {
            $('#tblHours input[name="ctl00$ContentPlaceHolder1$Hours"][value="rdbHExpAny"]').prop('checked', true);
            $("#ctl00_ContentPlaceHolder1_txtHExpEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtHExpEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtHExpGreaterAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtHExpGreaterAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtHExpLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtHExpLessAndEqual").attr("disabled", true);
        }
        else if ((filterValue.indexOf(">=") != -1 || filterValue.indexOf("&gt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#ctl00_ContentPlaceHolder1_txtHExpGreaterAndEqual").attr("disabled", false);
            $('#tblHours input[name="ctl00$ContentPlaceHolder1$Hours"][value="rdbHExpGreaterAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtHExpGreaterAndEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtHExpEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtHExpEqual").attr("disabled", 'disabled');
            $("#ctl00_ContentPlaceHolder1_txtHExpLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtHExpLessAndEqual").attr("disabled", 'disabled');
        }
        else if ((filterValue.indexOf("<=") != -1 || filterValue.indexOf("&lt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#ctl00_ContentPlaceHolder1_txtHExpLessAndEqual").attr("disabled", false);
            $('#tblHours input[name="ctl00$ContentPlaceHolder1$Hours"][value="rdbHExpLessAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtHExpLessAndEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtHExpEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtHExpEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtHExpGreaterAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtHExpGreaterAndEqual").attr("disabled", true);
        }
        else if (filterValue.indexOf('and') != -1) {
            var splitedExpenses = '';
            var greaterValue = '';
            var lessValue = '';
            splitedExpenses = filterValue.split('and');
            greaterValue = splitedExpenses[0].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            lessValue = splitedExpenses[1].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtHExpGreaterAndEqual").val(greaterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtHExpLessAndEqual").val(lessValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtHExpEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtHExpEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtHExpGreaterAndEqual").attr("disabled", false);
            $("#ctl00_ContentPlaceHolder1_txtHExpLessAndEqual").attr("disabled", false);
            $('#tblHours input[name="ctl00$ContentPlaceHolder1$Hours"][value="rdbHExpLessAndEqual"]').prop('checked', true);
        }
        else {
            $('#tblHours input[name="ctl00$ContentPlaceHolder1$Hours"][value="rdbHExpEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtHExpEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtHExpGreaterAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtHExpGreaterAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtHExpLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtHExpLessAndEqual").attr("disabled", true);

        }

        //                filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
        //                $("#ctl00_ContentPlaceHolder1_txtBalance").val(filterValue.trim());
    }
    //Expenses

    if (filterName == "loc") {
        if (filterValue == "") {
            $('#tblLoc input[name="ctl00$ContentPlaceHolder1$loc"][value="rdbLocAny"]').prop('checked', true);
            $("#ctl00_ContentPlaceHolder1_txtLocEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtLocEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").attr("disabled", true);
        }
            //else if ((filterValue.contains(">=") || filterValue.contains("&gt;")) && !filterValue.contains('and')) {
        else if ((filterValue.indexOf(">=") != -1 || filterValue.indexOf("&gt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").attr("disabled", false);
            $('#tblLoc input[name="ctl00$ContentPlaceHolder1$loc"][value="rdbLocGreaterAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtLocEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtLocEqual").attr("disabled", 'disabled');
            $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").attr("disabled", 'disabled');
        }
        else if ((filterValue.indexOf("<=") != -1 || filterValue.indexOf("&lt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").attr("disabled", false);
            $('#tblLoc input[name="ctl00$ContentPlaceHolder1$loc"][value="rdbLocLessAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtLocEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtLocEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").attr("disabled", true);
        }
        else if (filterValue.indexOf('and') != -1) {
            var splitedLoc = '';
            var greaterValue = '';
            var lessValue = '';
            splitedLoc = filterValue.split('and');
            greaterValue = splitedLoc[0].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            lessValue = splitedLoc[1].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").val(greaterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").val(lessValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtLocEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtLocEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").attr("disabled", false);
            $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").attr("disabled", false);
            $('#tblLoc input[name="ctl00$ContentPlaceHolder1$loc"][value="rdbLocLessAndEqual"]').prop('checked', true);
        }
        else {
            $('#tblLoc input[name="ctl00$ContentPlaceHolder1$loc"][value="rdbLocEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtLocEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").attr("disabled", true);

        }
    }

    if (filterName == "equip") {
        if (filterValue == "") {
            $('#tblEquip input[name="ctl00$ContentPlaceHolder1$equip"][value="rdbEquipAny"]').prop('checked', true);
            $("#ctl00_ContentPlaceHolder1_txtEquipEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").attr("disabled", true);
        }
        else if ((filterValue.indexOf(">=") != -1 || filterValue.indexOf("&gt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").attr("disabled", false);
            $('#tblEquip input[name="ctl00$ContentPlaceHolder1$equip"][value="rdbEquipGreaterAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtEquipEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipEqual").attr("disabled", 'disabled');
            $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").attr("disabled", 'disabled');
        }
        else if ((filterValue.indexOf("<=") != -1 || filterValue.indexOf("&lt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").attr("disabled", false);
            $('#tblEquip input[name="ctl00$ContentPlaceHolder1$equip"][value="rdbEquipLessAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtEquipEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").attr("disabled", true);
        }
        else if (filterValue.indexOf('and') != -1) {
            var splitedEquip = '';
            var greaterValue = '';
            var lessValue = '';
            splitedEquip = filterValue.split('and');
            greaterValue = splitedEquip[0].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            lessValue = splitedEquip[1].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").val(greaterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").val(lessValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtEquipEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").attr("disabled", false);
            $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").attr("disabled", false);
            $('#tblEquip input[name="ctl00$ContentPlaceHolder1$equip"][value="rdbEquipLessAndEqual"]').prop('checked', true);
        }
        else {
            $('#tblEquip input[name="ctl00$ContentPlaceHolder1$equip"][value="rdbEquipEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtEquipEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").attr("disabled", true);

        }
    }

    if (filterName == "EquipmentCounts") {
        if (filterValue == "") {
            $('#tblEquipmentCounts input[name="ctl00$ContentPlaceHolder1$equipmentcounts"][value="rdbEquipmentCountsAny"]').prop('checked', true);
            $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsLessAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsGreatAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsGreatAndEqual").attr("disabled", true);
        }
        else if ((filterValue.indexOf(">=") != -1 || filterValue.indexOf("&gt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsGreatAndEqual").attr("disabled", false);
            $('#tblEquipmentCounts input[name="ctl00$ContentPlaceHolder1$equipmentcounts"][value="rdbEquipmentCountsGreaterAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsGreatAndEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsEqual").attr("disabled", 'disabled');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsLessAndEqual").attr("disabled", 'disabled');
        }
        else if ((filterValue.indexOf("<=") != -1 || filterValue.indexOf("&lt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsLessAndEqual").attr("disabled", false);
            $('#tblEquipmentCounts input[name="ctl00$ContentPlaceHolder1$equipmentcounts"][value="rdbEquipmentCountsLessAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsLessAndEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsGreatAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsGreatAndEqual").attr("disabled", true);
        }
        else if (filterValue.indexOf('and') != -1) {
            var splitedEquip = '';
            var greaterValue = '';
            var lessValue = '';
            splitedEquip = filterValue.split('and');
            greaterValue = splitedEquip[0].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            lessValue = splitedEquip[1].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsGreatAndEqual").val(greaterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsLessAndEqual").val(lessValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsGreatAndEqual").attr("disabled", false);
            $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsLessAndEqual").attr("disabled", false);
            $('#tblEquipmentCounts input[name="ctl00$ContentPlaceHolder1$equipmentcounts"][value="rdbEquipmentCountsLessAndEqual"]').prop('checked', true);
        }
        else {
            $('#tblEquipmentCounts input[name="ctl00$ContentPlaceHolder1$equipmentcounts"][value="rdbEquipmentCountsEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsGreatAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsGreatAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsLessAndEqual").attr("disabled", true);

        }
    }

    if (filterName == "opencall") {
        if (filterValue == "") {
            $('#tblOpenCalls input[name="ctl00$ContentPlaceHolder1$oc"][value="rdbOCAny"]').prop('checked', true);
            $("#ctl00_ContentPlaceHolder1_txtOCEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtOCEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").attr("disabled", true);
        }
        else if ((filterValue.indexOf(">=") != -1 || filterValue.indexOf("&gt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").attr("disabled", false);
            $('#tblOpenCalls input[name="ctl00$ContentPlaceHolder1$oc"][value="rdbOCGreaterAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtOCEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtOCEqual").attr("disabled", 'disabled');
            $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").attr("disabled", 'disabled');
        }
        else if ((filterValue.indexOf("<=") != -1 || filterValue.indexOf("&lt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").attr("disabled", false);
            $('#tblOpenCalls input[name="ctl00$ContentPlaceHolder1$oc"][value="rdbOCLessAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtOCEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtOCEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").attr("disabled", true);
        }
        else if (filterValue.indexOf('and') != -1) {
            var splitedOC = '';
            var greaterValue = '';
            var lessValue = '';
            splitedOC = filterValue.split('and');
            greaterValue = splitedOC[0].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            lessValue = splitedOC[1].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").val(greaterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").val(lessValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtOCEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtOCEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").attr("disabled", false);
            $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").attr("disabled", false);
            $('#tblOpenCalls input[name="ctl00$ContentPlaceHolder1$oc"][value="rdbOCLessAndEqual"]').prop('checked', true);
        }
        else {
            $('#tblOpenCalls input[name="ctl00$ContentPlaceHolder1$oc"][value="rdbOCEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtOCEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").attr("disabled", true);

        }

    }

    if (filterName == "EquipmentPrice") {
        if (filterValue == "") {
            $('#tblEquipmentPrice input[name="ctl00$ContentPlaceHolder1$ep"][value="rdbEquipmentPriceAny"]').prop('checked', true);
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").attr("disabled", true);
        }
        else if ((filterValue.indexOf(">=") != -1 || filterValue.indexOf("&gt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").attr("disabled", false);
            $('#tblEquipmentPrice input[name="ctl00$ContentPlaceHolder1$ep"][value="rdbEquipmentPriceGreaterAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").attr("disabled", 'disabled');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").attr("disabled", 'disabled');
        }
        else if ((filterValue.indexOf("<=") != -1 || filterValue.indexOf("&lt;") != -1) && filterValue.indexOf('and') == -1) {
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").attr("disabled", false);
            $('#tblEquipmentPrice input[name="ctl00$ContentPlaceHolder1$ep"][value="rdbEquipmentPriceLessAndEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").attr("disabled", true);
        }
        else if (filterValue.indexOf('and') != -1) {
            var splitedOC = '';
            var greaterValue = '';
            var lessValue = '';
            splitedOC = filterValue.split('and');
            greaterValue = splitedOC[0].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            lessValue = splitedOC[1].replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").val(greaterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").val(lessValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").attr("disabled", false);
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").attr("disabled", false);
            $('#tblEquipmentPrice input[name="ctl00$ContentPlaceHolder1$ep"][value="rdbEquipmentPriceLessAndEqual"]').prop('checked', true);
        }
        else {
            $('#tblEquipmentPrice input[name="ctl00$ContentPlaceHolder1$ep"][value="rdbEquipmentPriceEqual"]').prop('checked', true);
            filterValue = filterValue.replace("&gt;=", '').replace(">=", '').replace("&lt;=", '').replace("<=", '').replace("=", '');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").val(filterValue.trim());
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").attr("disabled", true);

        }

    }


    if (filterName == "Status") {
        filterValue == "" ? ($("#ctl00_ContentPlaceHolder1_drpStatus").val('All')) : ($("#ctl00_ContentPlaceHolder1_drpStatus  option:contains('" + filterValue + "')").prop('selected', true));
    }


    //Rahil
    //  if (filterName == "LocationId") {
    // filterValue == "" ? ($("#ctl00_ContentPlaceHolder1_drpType").val('All')) : ($("#ctl00_ContentPlaceHolder1_drpType option:contains('" + filterValue + "')").prop('selected', true));

    if (filterName == "Date Created") {

        //$("#ctl00_ContentPlaceHolder1_txtName").val(filterValue);
        //                filterValue == "" ? ($("#ctl00_ContentPlaceHolder1_drpName").val('All')) : ($("#ctl00_ContentPlaceHolder1_drpName option:contains('" + filterValue + "')").prop('selected', true));
        // filterValue == "" ? ($("#ctl00_ContentPlaceHolder1_drpName").val('All')) : ($("#ctl00_ContentPlaceHolder1_drpName").val(filterValue));
        $('#ctl00_ContentPlaceHolder1_drpDateCreated input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#ctl00_ContentPlaceHolder1_drpDateCreated input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#ctl00_ContentPlaceHolder1_drpDateCreated input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "Location Name") {

        $('#ctl00_ContentPlaceHolder1_drpLocationName input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#ctl00_ContentPlaceHolder1_drpLocationName input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#ctl00_ContentPlaceHolder1_drpLocationName input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "Total Billed") {

        $('#ctl00_ContentPlaceHolder1_drpTotalBilled input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#ctl00_ContentPlaceHolder1_drpTotalBilled input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#ctl00_ContentPlaceHolder1_drpTotalBilled input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "Pre Tax Amount") {

        $('#ctl00_ContentPlaceHolder1_drpPreTaxAmount input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#ctl00_ContentPlaceHolder1_drpPreTaxAmount input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#ctl00_ContentPlaceHolder1_drpPreTaxAmount input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "Sales Tax") {

        $('#ctl00_ContentPlaceHolder1_drpSalesTax input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#ctl00_ContentPlaceHolder1_drpSalesTax input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#ctl00_ContentPlaceHolder1_drpSalesTax input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "Customer Name") {

        $('#ctl00_ContentPlaceHolder1_drpCustomerName input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#ctl00_ContentPlaceHolder1_drpCustomerName input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#ctl00_ContentPlaceHolder1_drpCustomerName input[type="checkbox"]').attr('checked', false);
        }
    }

    //if (filterName == "Department Type") {

    //    $('#ctl00_ContentPlaceHolder1_drpType input[type="checkbox"]').attr('checked', false);
    //    if (filterValue != '') {
    //        var splitedNames = filterValue.split("|");
    //        for (i = 0; i <= splitedNames.length - 1; i++) {
    //            $('#ctl00_ContentPlaceHolder1_drpType input[type=checkbox]').each(function () {
    //                var lblValue = $(this).next('label').html();
    //                splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
    //                if (lblValue.trim() == splitedNames[i].trim()) {
    //                    $(this).attr('checked', true);
    //                }
    //            });
    //        }
    //    }
    //    else {
    //        $('#ctl00_ContentPlaceHolder1_drpType input[type="checkbox"]').attr('checked', false);
    //    }
    //}

    if (filterName == "Total On Order") {

        $('#ctl00_ContentPlaceHolder1_drpTotalOnOrder input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#ctl00_ContentPlaceHolder1_drpTotalOnOrder input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#ctl00_ContentPlaceHolder1_drpTotalOnOrder input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "Due Date") {

        $('#ctl00_ContentPlaceHolder1_drpDueDate input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#ctl00_ContentPlaceHolder1_drpDueDate input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#ctl00_ContentPlaceHolder1_drpDueDate input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "Amount Due") {

        $('#ctl00_ContentPlaceHolder1_drpAmountDue input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#ctl00_ContentPlaceHolder1_drpAmountDue input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#ctl00_ContentPlaceHolder1_drpAmountDue input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "Manual Invoice") {

        $('#ctl00_ContentPlaceHolder1_drpManualInvoice input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#ctl00_ContentPlaceHolder1_drpManualInvoice input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#ctl00_ContentPlaceHolder1_drpManualInvoice input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "Due Date") {

        $('#ctl00_ContentPlaceHolder1_drpDueDate input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedNames = filterValue.split("|");
            for (i = 0; i <= splitedNames.length - 1; i++) {
                $('#ctl00_ContentPlaceHolder1_drpDueDate input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedNames[i] = splitedNames[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedNames[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#ctl00_ContentPlaceHolder1_drpDueDate input[type="checkbox"]').attr('checked', false);
        }
    }

    if (filterName == "Customer" || filterName == "Location" || filterName == "Description" || filterName == "Total" || filterName == "Hours" || filterName == "Project") {

        $('#ctl00_ContentPlaceHolder1_drp' + filterName + ' input[type="checkbox"]').attr('checked', false);
        if (filterValue != '') {
            var splitedValues = filterValue.split("|");
            for (i = 0; i <= splitedValues.length - 1; i++) {
                $('#ctl00_ContentPlaceHolder1_drp' + filterName + ' input[type=checkbox]').each(function () {
                    var lblValue = $(this).next('label').html();
                    splitedValues[i] = splitedValues[i].replace("'", "").replace("'", "");
                    if (lblValue.trim() == splitedValues[i].trim()) {
                        $(this).attr('checked', true);
                    }
                });
            }
        }
        else {
            $('#ctl00_ContentPlaceHolder1_drp' + filterName + ' input[type="checkbox"]').attr('checked', false);
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

    if ($("#ctl00_ContentPlaceHolder1_hdnMainHeader").val() == 'True') {
        $("#dvMainHeader").show();
    }
    else {
        $("#dvMainHeader").hide();
    }
    var getColumnWidth = $("#ctl00_ContentPlaceHolder1_hdnColumnWidth").val();
    var splittedColWidth = '';
    if (getColumnWidth != '') {
        splittedColWidth = $("#ctl00_ContentPlaceHolder1_hdnColumnWidth").val().split(",");

        $('#tblResize tr th').each(function (index, element) {
            var colWidth = splittedColWidth[index];
            $(this).css('width', colWidth);
        });
    }

    var columnList = $("#ctl00_ContentPlaceHolder1_hdnColumnList").val();
    $('#ctl00_ContentPlaceHolder1_lstColumnSort option').remove();
    var splitedColList = columnList.split(",");
    $.each(splitedColList, function (key, value) {

        $('#ctl00_ContentPlaceHolder1_lstColumnSort')
                                            .append($("<option></option>")
                                 .attr("value", value)
                                 .text(value));

    });

    //$("#ctl00_ContentPlaceHolder1_lstFilter option[value='loc'],option[value='equip'], option[value='opencall']").remove();

    if ($("#ctl00_ContentPlaceHolder1_drpReports").val() == 0) {
        $("#btnCustomizeReport").hide();
        $("#ctl00_ContentPlaceHolder1_btnDeleteReport").hide();
    }
    else {
        $("#btnCustomizeReport").show();
        $("#ctl00_ContentPlaceHolder1_btnDeleteReport").show();
    }


    $('[name="ctl00$ContentPlaceHolder1$CrystalReportViewer1$ctl02$ctl11"]').hide();
    $('[name="ctl00$ContentPlaceHolder1$CrystalReportViewer1$ctl02$ctl03"]').hide();

    jQuery('.tabs .tab-links a').on('click', function (e) {
        var currentAttrValue = jQuery(this).attr('href');

        // Show/Hide Tabs
        jQuery('.tabs ' + currentAttrValue).show().siblings().hide();

        // Change/remove current tab to active
        jQuery(this).parent('li').addClass('active').siblings().removeClass('active');

        e.preventDefault();
    });

    $("#btnPrint").click(function () {
        $("#ctl00_ContentPlaceHolder1_dvGridReport").print();
        return (false);
    });

    $("#btnApply, #btnPreviewApply").click(function () {
        var isChecked = null;
        var chkArray = [];

        $('#ctl00_ContentPlaceHolder1_chkColumnList input[type=checkbox]:checked').each(function () {
            isChecked = true;
            //chkArray.push($(this).parent().find('label').html());
        });
        if (isChecked) {

            $("#popup").hide();
            $("#ctl00_ContentPlaceHolder1_dvSaveReport").show();
            var drpSortByVal = $('#ctl00_ContentPlaceHolder1_drpSortBy option:selected').val();
            $("#ctl00_ContentPlaceHolder1_hdnDrpSortBy").val(drpSortByVal);

        }
        else {
            $("#ctl00_ContentPlaceHolder1_dvSaveReport").hide();
            alert("Please select at least one field!");
            return;
        }

        var lstSortColumn = '';
        $('#ctl00_ContentPlaceHolder1_lstColumnSort option').each(function (index, element) {
            //alert($(element).val());
            lstSortColumn += $(element).val() + "^";

        });

        $("#ctl00_ContentPlaceHolder1_hdnLstColumns").val(lstSortColumn);

        var lstColumnWidth = '';
        $('#tblResize tr th').each(function () {
            lstColumnWidth += $(this).css('width') + "^";
        });

        $("#ctl00_ContentPlaceHolder1_hdnColumnWidth").val(lstColumnWidth);

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
                        $("#ctl00_ContentPlaceHolder1_ddlStateReference option:contains('" + actualText + "')").attr("selected", "selected")
                        var getValue = $("#ctl00_ContentPlaceHolder1_ddlStateReference option:selected").val();
                        filterValues += "'" + getValue + "'" + "|";
                    }
                    filterValues = filterValues.substring(0, filterValues.length - 1) + "^";
                }
            }
        });

        $("#ctl00_ContentPlaceHolder1_hdnFilterColumns").val(filterColumns);
        $("#ctl00_ContentPlaceHolder1_hdnFilterValues").val(filterValues);

    });

    //Added By Yashasvi Jadav
    $("#btnPreview").click(function () {
        $("#myModal").modal('show');

        $('#ctl00_ContentPlaceHolder1_lblPreviewCompEmail').text("");
        $('#ctl00_ContentPlaceHolder1_lblPreviewCompanyName').text("");
        $('#ctl00_ContentPlaceHolder1_lblPreviewCompAddress').text("");
        $('#ctl00_ContentPlaceHolder1_imgPreview').attr('src', "");
        $('#ctl00_ContentPlaceHolder1_lblPreviewTime').text("");
        $('#ctl00_ContentPlaceHolder1_lblPreviewDate').text("");

        var isChecked = null;
        var chkArray = [];
        var drpSortByVal = null;

        $('#ctl00_ContentPlaceHolder1_chkColumnList input[type=checkbox]:checked').each(function () {
            isChecked = true;
        });
        if (isChecked) {

            $("#popup").hide();
            //$("#ctl00_ContentPlaceHolder1_dvSaveReport").show();
            drpSortByVal = $('#ctl00_ContentPlaceHolder1_drpSortBy option:selected').val();
            //$("#ctl00_ContentPlaceHolder1_hdnDrpSortBy").val(drpSortByVal);

        }
        else {
            $("#ctl00_ContentPlaceHolder1_dvSaveReport").hide();
            alert("Please select at least one field!");
            return;
        }

        var lstSortColumn = '';
        $('#ctl00_ContentPlaceHolder1_lstColumnSort option').each(function (index, element) {
            lstSortColumn += $(element).val() + "^";

        });

        //$("#ctl00_ContentPlaceHolder1_hdnLstColumns").val(lstSortColumn);

        var lstColumnWidth = '';
        $('#tblResize tr th').each(function () {
            lstColumnWidth += $(this).css('width') + "^";
        });

        //$("#ctl00_ContentPlaceHolder1_hdnColumnWidth").val(lstColumnWidth);

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
                        $("#ctl00_ContentPlaceHolder1_ddlStateReference option:contains('" + actualText + "')").attr("selected", "selected")
                        var getValue = $("#ctl00_ContentPlaceHolder1_ddlStateReference option:selected").val();
                        filterValues += "'" + getValue + "'" + "|";
                    }
                    filterValues = filterValues.substring(0, filterValues.length - 1) + "^";
                }
            }
        });

        //$("#ctl00_ContentPlaceHolder1_hdnFilterColumns").val(filterColumns);
        //$("#ctl00_ContentPlaceHolder1_hdnFilterValues").val(filterValues);

        var reportId = $("#ctl00_ContentPlaceHolder1_drpReports").find('option:selected').val();
        $('#loading').show();
        $("#tblPreviewReport").empty();
        $.ajax({
            type: "POST",
            url: "ProjectListingReport.aspx/GetProjectPreviewDetails",
            data: JSON.stringify({ reportId: reportId, FilterColumn: filterColumns, FilterValues: filterValues, ColumnWidth: lstColumnWidth, SortColumn: lstSortColumn, DataSortBy: drpSortByVal }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response != null || response != "") {
                    var _data = JSON.parse(response.d);
                    $('#ctl00_ContentPlaceHolder1_lblPreviewCompEmail').text(_data["Email"]);
                    $('#ctl00_ContentPlaceHolder1_lblPreviewCompanyName').text(_data["Name"]);
                    $('#ctl00_ContentPlaceHolder1_lblPreviewCompAddress').text(_data["Address"]);
                    $('#ctl00_ContentPlaceHolder1_imgPreview').attr({ 'src': _data["Image"], 'width': '150px', 'height': '150px' });
                    $('#ctl00_ContentPlaceHolder1_lblPreviewTime').text(_data["Time"]);
                    $('#ctl00_ContentPlaceHolder1_lblPreviewDate').text(_data["Date"]);
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
        $("#ctl00_ContentPlaceHolder1_drpSortBy option").remove();
        $('#ctl00_ContentPlaceHolder1_lstColumnSort option').remove();
        $("#ctl00_ContentPlaceHolder1_hdnReportAction").val('Save');
        $("INPUT[type='checkbox']").attr('checked', false);
        $("#ctl00_ContentPlaceHolder1_txtReportName").val('');
        $('#spnModelTitle').html('New Report: Project Report');

        var radio = $("[id*=rdbOrders] input[value=1]");
        radio.attr("checked", "checked");
        EmptyFilters();
        $("#tblFilterChoices tbody").empty();
        $("#ctl00_ContentPlaceHolder1_lstFilter").val('Name');
        $("#tblName").fadeIn(200).css("display", "block");
        //                $('#tblBalance input[name="ctl00$ContentPlaceHolder1$Balance"][value="rdbAny"]').prop('checked', true);
        //                $("#ctl00_ContentPlaceHolder1_txtBalance").attr("disabled", "disabled");

        $("#ctl00_ContentPlaceHolder1_chkMainHeader").attr("checked", true);
        $("#ctl00_ContentPlaceHolder1_chkDatePrepared").attr("checked", true);
        $("#ctl00_ContentPlaceHolder1_chkTimePrepared").attr("checked", true);

    });



    $('#tblBalance input[name="ctl00$ContentPlaceHolder1$Balance"] + label').click(function () {
        if ($(this).text() != "Any") {
            if ($(this).text() == "=") {
                if ($("#ctl00_ContentPlaceHolder1_txtBalEqual").val() == '') {
                    $("#trBalance").remove();
                }
                $("#ctl00_ContentPlaceHolder1_txtBalEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").attr("disabled", true);
                $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").attr("disabled", true);
                //                        if ($("#ctl00_ContentPlaceHolder1_txtBalEqual").val() != '') {
                //                            var replaceOperator = $(".balance").html();
                //                            replaceOperator = replaceOperator.replace(">=", '').replace("&gt;=", '').replace("<=", '').replace("&lt;=", '').replace("=", '');
                //                            $(".balance").html($(this).text() + replaceOperator);
                //                        }
                //                        else {
                //                            $("#trBalance").remove();
                //                        }
            }
            else if ($(this).text() == ">=" || $(this).text() == "&gt;") {
                $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtBalEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtBalEqual").attr("disabled", true);
                if ($("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").val() == '') {
                    $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").attr("disabled", true);
                }
                else {
                    $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").attr("disabled", false);
                }
                //                        if ($("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").val() != '') {
                //                            var replaceOperator = $(".balance").html();
                //                            replaceOperator = replaceOperator.replace(">=", '').replace("&gt;=", '').replace("<=", '').replace("&lt;=", '').replace("=", '');
                //                            $(".balance").html($(this).text() + replaceOperator);
                //                        }
                //                        else {
                //                            $("#trBalance").remove();
                //                        }
            }
            else if ($(this).text() == "<=" || $(this).text() == "&lt;") {
                $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtBalEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtBalEqual").attr("disabled", true);
                if ($("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").val() == '') {
                    $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").attr("disabled", true);
                }
                else {
                    $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").attr("disabled", false);
                }
                //                        if ($("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").val() != '') {
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
            //                    $("#ctl00_ContentPlaceHolder1_txtBalance").val('');
            //                    $("#ctl00_ContentPlaceHolder1_txtBalance").attr("disabled", true);

            $("#ctl00_ContentPlaceHolder1_txtBalEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtBalEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").attr("disabled", true);
            $("#trBalance").remove();

        }
    });

    //Expenses
    $('#tblExpenses input[name="ctl00$ContentPlaceHolder1$Expenses"] + label').click(function () {
        if ($(this).text() != "Any") {
            if ($(this).text() == "=") {
                if ($("#ctl00_ContentPlaceHolder1_txtExpEqual").val() == '') {
                    $("#trExpenses").remove();
                }
                $("#ctl00_ContentPlaceHolder1_txtExpEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtExpGreaterAndEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtExpGreaterAndEqual").attr("disabled", true);
                $("#ctl00_ContentPlaceHolder1_txtExpLessAndEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtExpLessAndEqual").attr("disabled", true);
                //                        if ($("#ctl00_ContentPlaceHolder1_txtBalEqual").val() != '') {
                //                            var replaceOperator = $(".balance").html();
                //                            replaceOperator = replaceOperator.replace(">=", '').replace("&gt;=", '').replace("<=", '').replace("&lt;=", '').replace("=", '');
                //                            $(".balance").html($(this).text() + replaceOperator);
                //                        }
                //                        else {
                //                            $("#trBalance").remove();
                //                        }
            }
            else if ($(this).text() == ">=" || $(this).text() == "&gt;") {
                $("#ctl00_ContentPlaceHolder1_txtExpGreaterAndEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtExpEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtExpEqual").attr("disabled", true);
                if ($("#ctl00_ContentPlaceHolder1_txtExpLessAndEqual").val() == '') {
                    $("#ctl00_ContentPlaceHolder1_txtExpLessAndEqual").attr("disabled", true);
                }
                else {
                    $("#ctl00_ContentPlaceHolder1_txtExpLessAndEqual").attr("disabled", false);
                }
                //                        if ($("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").val() != '') {
                //                            var replaceOperator = $(".balance").html();
                //                            replaceOperator = replaceOperator.replace(">=", '').replace("&gt;=", '').replace("<=", '').replace("&lt;=", '').replace("=", '');
                //                            $(".balance").html($(this).text() + replaceOperator);
                //                        }
                //                        else {
                //                            $("#trBalance").remove();
                //                        }
            }
            else if ($(this).text() == "<=" || $(this).text() == "&lt;") {
                $("#ctl00_ContentPlaceHolder1_txtExpLessAndEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtExpEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtExpEqual").attr("disabled", true);
                if ($("#ctl00_ContentPlaceHolder1_txtExpGreaterAndEqual").val() == '') {
                    $("#ctl00_ContentPlaceHolder1_txtExpGreaterAndEqual").attr("disabled", true);
                }
                else {
                    $("#ctl00_ContentPlaceHolder1_txtExpGreaterAndEqual").attr("disabled", false);
                }
                //                        if ($("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").val() != '') {
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
            //                    $("#ctl00_ContentPlaceHolder1_txtBalance").val('');
            //                    $("#ctl00_ContentPlaceHolder1_txtBalance").attr("disabled", true);

            $("#ctl00_ContentPlaceHolder1_txtExpEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtExpEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtExpLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtExpLessAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtExpGreaterAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtExpGreaterAndEqual").attr("disabled", true);
            $("#trExpenses").remove();

        }
    });
    
    $('#tblLaborExpense input[name="ctl00$ContentPlaceHolder1$LaborExpense"] + label').click(function () {
        if ($(this).text() != "Any") {
            if ($(this).text() == "=") {
                if ($("#ctl00_ContentPlaceHolder1_txtLExpEqual").val() == '') {
                    $("#trLaborExpense").remove();
                }
                $("#ctl00_ContentPlaceHolder1_txtLExpEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtLExpGreaterAndEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtLExpGreaterAndEqual").attr("disabled", true);
                $("#ctl00_ContentPlaceHolder1_txtLExpLessAndEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtLExpLessAndEqual").attr("disabled", true);
                //                        if ($("#ctl00_ContentPlaceHolder1_txtBalEqual").val() != '') {
                //                            var replaceOperator = $(".balance").html();
                //                            replaceOperator = replaceOperator.replace(">=", '').replace("&gt;=", '').replace("<=", '').replace("&lt;=", '').replace("=", '');
                //                            $(".balance").html($(this).text() + replaceOperator);
                //                        }
                //                        else {
                //                            $("#trBalance").remove();
                //                        }
            }
            else if ($(this).text() == ">=" || $(this).text() == "&gt;") {
                $("#ctl00_ContentPlaceHolder1_txtLExpGreaterAndEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtLExpEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtLExpEqual").attr("disabled", true);
                if ($("#ctl00_ContentPlaceHolder1_txtLExpLessAndEqual").val() == '') {
                    $("#ctl00_ContentPlaceHolder1_txtLExpLessAndEqual").attr("disabled", true);
                }
                else {
                    $("#ctl00_ContentPlaceHolder1_txtLExpLessAndEqual").attr("disabled", false);
                }
                //                        if ($("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").val() != '') {
                //                            var replaceOperator = $(".balance").html();
                //                            replaceOperator = replaceOperator.replace(">=", '').replace("&gt;=", '').replace("<=", '').replace("&lt;=", '').replace("=", '');
                //                            $(".balance").html($(this).text() + replaceOperator);
                //                        }
                //                        else {
                //                            $("#trBalance").remove();
                //                        }
            }
            else if ($(this).text() == "<=" || $(this).text() == "&lt;") {
                $("#ctl00_ContentPlaceHolder1_txtLExpLessAndEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtLExpEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtLExpEqual").attr("disabled", true);
                if ($("#ctl00_ContentPlaceHolder1_txtLExpGreaterAndEqual").val() == '') {
                    $("#ctl00_ContentPlaceHolder1_txtLExpGreaterAndEqual").attr("disabled", true);
                }
                else {
                    $("#ctl00_ContentPlaceHolder1_txtLExpGreaterAndEqual").attr("disabled", false);
                }
                //                        if ($("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").val() != '') {
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
            //                    $("#ctl00_ContentPlaceHolder1_txtBalance").val('');
            //                    $("#ctl00_ContentPlaceHolder1_txtBalance").attr("disabled", true);

            $("#ctl00_ContentPlaceHolder1_txtLExpEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtLExpEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtLExpLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtLExpLessAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtLExpGreaterAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtLExpGreaterAndEqual").attr("disabled", true);
            $("#trLaborExpense").remove();

        }
    });

    $('#tblTotalExpenses input[name="ctl00$ContentPlaceHolder1$TotalExpenses"] + label').click(function () {
        if ($(this).text() != "Any") {
            if ($(this).text() == "=") {
                if ($("#ctl00_ContentPlaceHolder1_txtTExpEqual").val() == '') {
                    $("#trTotalExpenses").remove();
                }
                $("#ctl00_ContentPlaceHolder1_txtTExpEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtTExpGreaterAndEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtTExpGreaterAndEqual").attr("disabled", true);
                $("#ctl00_ContentPlaceHolder1_txtTExpLessAndEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtTExpLessAndEqual").attr("disabled", true);
                //                        if ($("#ctl00_ContentPlaceHolder1_txtBalEqual").val() != '') {
                //                            var replaceOperator = $(".balance").html();
                //                            replaceOperator = replaceOperator.replace(">=", '').replace("&gt;=", '').replace("<=", '').replace("&lt;=", '').replace("=", '');
                //                            $(".balance").html($(this).text() + replaceOperator);
                //                        }
                //                        else {
                //                            $("#trBalance").remove();
                //                        }
            }
            else if ($(this).text() == ">=" || $(this).text() == "&gt;") {
                $("#ctl00_ContentPlaceHolder1_txtTExpGreaterAndEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtTExpEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtTExpEqual").attr("disabled", true);
                if ($("#ctl00_ContentPlaceHolder1_txtTExpLessAndEqual").val() == '') {
                    $("#ctl00_ContentPlaceHolder1_txtTExpLessAndEqual").attr("disabled", true);
                }
                else {
                    $("#ctl00_ContentPlaceHolder1_txtTExpLessAndEqual").attr("disabled", false);
                }
                //                        if ($("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").val() != '') {
                //                            var replaceOperator = $(".balance").html();
                //                            replaceOperator = replaceOperator.replace(">=", '').replace("&gt;=", '').replace("<=", '').replace("&lt;=", '').replace("=", '');
                //                            $(".balance").html($(this).text() + replaceOperator);
                //                        }
                //                        else {
                //                            $("#trBalance").remove();
                //                        }
            }
            else if ($(this).text() == "<=" || $(this).text() == "&lt;") {
                $("#ctl00_ContentPlaceHolder1_txtTExpLessAndEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtTExpEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtTExpEqual").attr("disabled", true);
                if ($("#ctl00_ContentPlaceHolder1_txtTExpGreaterAndEqual").val() == '') {
                    $("#ctl00_ContentPlaceHolder1_txtTExpGreaterAndEqual").attr("disabled", true);
                }
                else {
                    $("#ctl00_ContentPlaceHolder1_txtTExpGreaterAndEqual").attr("disabled", false);
                }
                //                        if ($("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").val() != '') {
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
            //                    $("#ctl00_ContentPlaceHolder1_txtBalance").val('');
            //                    $("#ctl00_ContentPlaceHolder1_txtBalance").attr("disabled", true);

            $("#ctl00_ContentPlaceHolder1_txtTExpEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtTExpEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtTExpLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtTExpLessAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtTExpGreaterAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtTExpGreaterAndEqual").attr("disabled", true);
            $("#trTotalExpenses").remove();

        }
    });

    $('#tblTotalOnOrder input[name="ctl00$ContentPlaceHolder1$TotalOnOrder"] + label').click(function () {
        if ($(this).text() != "Any") {
            if ($(this).text() == "=") {
                if ($("#ctl00_ContentPlaceHolder1_txtTOExpEqual").val() == '') {
                    $("#trTotalOnOrder").remove();
                }
                $("#ctl00_ContentPlaceHolder1_txtTOExpEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtTOExpGreaterAndEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtTOExpGreaterAndEqual").attr("disabled", true);
                $("#ctl00_ContentPlaceHolder1_txtTOExpLessAndEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtTOExpLessAndEqual").attr("disabled", true);
                //                        if ($("#ctl00_ContentPlaceHolder1_txtBalEqual").val() != '') {
                //                            var replaceOperator = $(".balance").html();
                //                            replaceOperator = replaceOperator.replace(">=", '').replace("&gt;=", '').replace("<=", '').replace("&lt;=", '').replace("=", '');
                //                            $(".balance").html($(this).text() + replaceOperator);
                //                        }
                //                        else {
                //                            $("#trBalance").remove();
                //                        }
            }
            else if ($(this).text() == ">=" || $(this).text() == "&gt;") {
                $("#ctl00_ContentPlaceHolder1_txtTOExpGreaterAndEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtTOExpEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtTOExpEqual").attr("disabled", true);
                if ($("#ctl00_ContentPlaceHolder1_txtTOExpLessAndEqual").val() == '') {
                    $("#ctl00_ContentPlaceHolder1_txtTOExpLessAndEqual").attr("disabled", true);
                }
                else {
                    $("#ctl00_ContentPlaceHolder1_txtTOExpLessAndEqual").attr("disabled", false);
                }
                //                        if ($("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").val() != '') {
                //                            var replaceOperator = $(".balance").html();
                //                            replaceOperator = replaceOperator.replace(">=", '').replace("&gt;=", '').replace("<=", '').replace("&lt;=", '').replace("=", '');
                //                            $(".balance").html($(this).text() + replaceOperator);
                //                        }
                //                        else {
                //                            $("#trBalance").remove();
                //                        }
            }
            else if ($(this).text() == "<=" || $(this).text() == "&lt;") {
                $("#ctl00_ContentPlaceHolder1_txtTOExpLessAndEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtTOExpEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtTOExpOEqual").attr("disabled", true);
                if ($("#ctl00_ContentPlaceHolder1_txtTOExpGreaterAndEqual").val() == '') {
                    $("#ctl00_ContentPlaceHolder1_txtTOExpGreaterAndEqual").attr("disabled", true);
                }
                else {
                    $("#ctl00_ContentPlaceHolder1_txtTOExpGreaterAndEqual").attr("disabled", false);
                }
                //                        if ($("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").val() != '') {
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
            //                    $("#ctl00_ContentPlaceHolder1_txtBalance").val('');
            //                    $("#ctl00_ContentPlaceHolder1_txtBalance").attr("disabled", true);

            $("#ctl00_ContentPlaceHolder1_txtTOExpEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtTOExpEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtTOExpLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtTOExpLessAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtTOExpGreaterAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtTOExpGreaterAndEqual").attr("disabled", true);
            $("#trTotalOnOrder").remove();

        }
    });


    $('#tblMaterialExpense input[name="ctl00$ContentPlaceHolder1$MaterialExpense"] + label').click(function () {
        if ($(this).text() != "Any") {
            if ($(this).text() == "=") {
                if ($("#ctl00_ContentPlaceHolder1_txtMExpEqual").val() == '') {
                    $("#trMaterialExpense").remove();
                }
                $("#ctl00_ContentPlaceHolder1_txtMExpEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtMExpGreaterAndEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtMExpGreaterAndEqual").attr("disabled", true);
                $("#ctl00_ContentPlaceHolder1_txtMExpLessAndEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtMExpLessAndEqual").attr("disabled", true);
                //                        if ($("#ctl00_ContentPlaceHolder1_txtBalEqual").val() != '') {
                //                            var replaceOperator = $(".balance").html();
                //                            replaceOperator = replaceOperator.replace(">=", '').replace("&gt;=", '').replace("<=", '').replace("&lt;=", '').replace("=", '');
                //                            $(".balance").html($(this).text() + replaceOperator);
                //                        }
                //                        else {
                //                            $("#trBalance").remove();
                //                        }
            }
            else if ($(this).text() == ">=" || $(this).text() == "&gt;") {
                $("#ctl00_ContentPlaceHolder1_txtMExpGreaterAndEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtMExpEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtMExpEqual").attr("disabled", true);
                if ($("#ctl00_ContentPlaceHolder1_txtMExpLessAndEqual").val() == '') {
                    $("#ctl00_ContentPlaceHolder1_txtMExpLessAndEqual").attr("disabled", true);
                }
                else {
                    $("#ctl00_ContentPlaceHolder1_txtMExpLessAndEqual").attr("disabled", false);
                }
                //                        if ($("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").val() != '') {
                //                            var replaceOperator = $(".balance").html();
                //                            replaceOperator = replaceOperator.replace(">=", '').replace("&gt;=", '').replace("<=", '').replace("&lt;=", '').replace("=", '');
                //                            $(".balance").html($(this).text() + replaceOperator);
                //                        }
                //                        else {
                //                            $("#trBalance").remove();
                //                        }
            }
            else if ($(this).text() == "<=" || $(this).text() == "&lt;") {
                $("#ctl00_ContentPlaceHolder1_txtMExpLessAndEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtMExpEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtMExpEqual").attr("disabled", true);
                if ($("#ctl00_ContentPlaceHolder1_txtMExpGreaterAndEqual").val() == '') {
                    $("#ctl00_ContentPlaceHolder1_txtMExpGreaterAndEqual").attr("disabled", true);
                }
                else {
                    $("#ctl00_ContentPlaceHolder1_txtMExpGreaterAndEqual").attr("disabled", false);
                }
                //                        if ($("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").val() != '') {
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
            //                    $("#ctl00_ContentPlaceHolder1_txtBalance").val('');
            //                    $("#ctl00_ContentPlaceHolder1_txtBalance").attr("disabled", true);

            $("#ctl00_ContentPlaceHolder1_txtMExpEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtMExpEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtMExpLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtMExpLessAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtMExpGreaterAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtMExpGreaterAndEqual").attr("disabled", true);
            $("#trMaterialExpense").remove();

        }
    });

    $('#tblTotalBilled input[name="ctl00$ContentPlaceHolder1$TotalBilled"] + label').click(function () {
        if ($(this).text() != "Any") {
            if ($(this).text() == "=") {
                if ($("#ctl00_ContentPlaceHolder1_txtBExpEqual").val() == '') {
                    $("#trTotalBilled").remove();
                }
                $("#ctl00_ContentPlaceHolder1_txtBExpEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtBExpGreaterAndEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtBExpGreaterAndEqual").attr("disabled", true);
                $("#ctl00_ContentPlaceHolder1_txtBExpLessAndEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtBExpLessAndEqual").attr("disabled", true);
                //                        if ($("#ctl00_ContentPlaceHolder1_txtBalEqual").val() != '') {
                //                            var replaceOperator = $(".balance").html();
                //                            replaceOperator = replaceOperator.replace(">=", '').replace("&gt;=", '').replace("<=", '').replace("&lt;=", '').replace("=", '');
                //                            $(".balance").html($(this).text() + replaceOperator);
                //                        }
                //                        else {
                //                            $("#trBalance").remove();
                //                        }
            }
            else if ($(this).text() == ">=" || $(this).text() == "&gt;") {
                $("#ctl00_ContentPlaceHolder1_txtBExpGreaterAndEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtBExpEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtBExpEqual").attr("disabled", true);
                if ($("#ctl00_ContentPlaceHolder1_txtBExpLessAndEqual").val() == '') {
                    $("#ctl00_ContentPlaceHolder1_txtBExpLessAndEqual").attr("disabled", true);
                }
                else {
                    $("#ctl00_ContentPlaceHolder1_txtBExpLessAndEqual").attr("disabled", false);
                }
                //                        if ($("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").val() != '') {
                //                            var replaceOperator = $(".balance").html();
                //                            replaceOperator = replaceOperator.replace(">=", '').replace("&gt;=", '').replace("<=", '').replace("&lt;=", '').replace("=", '');
                //                            $(".balance").html($(this).text() + replaceOperator);
                //                        }
                //                        else {
                //                            $("#trBalance").remove();
                //                        }
            }
            else if ($(this).text() == "<=" || $(this).text() == "&lt;") {
                $("#ctl00_ContentPlaceHolder1_txtBExpLessAndEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtBExpEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtBExpEqual").attr("disabled", true);
                if ($("#ctl00_ContentPlaceHolder1_txtBExpGreaterAndEqual").val() == '') {
                    $("#ctl00_ContentPlaceHolder1_txtBExpGreaterAndEqual").attr("disabled", true);
                }
                else {
                    $("#ctl00_ContentPlaceHolder1_txtBExpGreaterAndEqual").attr("disabled", false);
                }
                //                        if ($("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").val() != '') {
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
            //                    $("#ctl00_ContentPlaceHolder1_txtBalance").val('');
            //                    $("#ctl00_ContentPlaceHolder1_txtBalance").attr("disabled", true);

            $("#ctl00_ContentPlaceHolder1_txtBExpEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtBExpEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtBExpLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtBExpLessAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtBExpGreaterAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtBExpGreaterAndEqual").attr("disabled", true);
            $("#trTotalBilled").remove();

        }
    });


    $('#tblHours input[name="ctl00$ContentPlaceHolder1$Hours"] + label').click(function () {
        if ($(this).text() != "Any") {
            if ($(this).text() == "=") {
                if ($("#ctl00_ContentPlaceHolder1_txtHExpEqual").val() == '') {
                    $("#trHours").remove();
                }
                $("#ctl00_ContentPlaceHolder1_txtHExpEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtHExpGreaterAndEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtHExpGreaterAndEqual").attr("disabled", true);
                $("#ctl00_ContentPlaceHolder1_txtHExpLessAndEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtHExpLessAndEqual").attr("disabled", true);
                //                        if ($("#ctl00_ContentPlaceHolder1_txtBalEqual").val() != '') {
                //                            var replaceOperator = $(".balance").html();
                //                            replaceOperator = replaceOperator.replace(">=", '').replace("&gt;=", '').replace("<=", '').replace("&lt;=", '').replace("=", '');
                //                            $(".balance").html($(this).text() + replaceOperator);
                //                        }
                //                        else {
                //                            $("#trBalance").remove();
                //                        }
            }
            else if ($(this).text() == ">=" || $(this).text() == "&gt;") {
                $("#ctl00_ContentPlaceHolder1_txtHExpGreaterAndEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtHExpEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtHExpEqual").attr("disabled", true);
                if ($("#ctl00_ContentPlaceHolder1_txtHExpLessAndEqual").val() == '') {
                    $("#ctl00_ContentPlaceHolder1_txtHExpLessAndEqual").attr("disabled", true);
                }
                else {
                    $("#ctl00_ContentPlaceHolder1_txtHExpLessAndEqual").attr("disabled", false);
                }
                //                        if ($("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").val() != '') {
                //                            var replaceOperator = $(".balance").html();
                //                            replaceOperator = replaceOperator.replace(">=", '').replace("&gt;=", '').replace("<=", '').replace("&lt;=", '').replace("=", '');
                //                            $(".balance").html($(this).text() + replaceOperator);
                //                        }
                //                        else {
                //                            $("#trBalance").remove();
                //                        }
            }
            else if ($(this).text() == "<=" || $(this).text() == "&lt;") {
                $("#ctl00_ContentPlaceHolder1_txtHExpLessAndEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtHExpEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtHExpEqual").attr("disabled", true);
                if ($("#ctl00_ContentPlaceHolder1_txtHExpGreaterAndEqual").val() == '') {
                    $("#ctl00_ContentPlaceHolder1_txtHExpGreaterAndEqual").attr("disabled", true);
                }
                else {
                    $("#ctl00_ContentPlaceHolder1_txtHExpGreaterAndEqual").attr("disabled", false);
                }
                //                        if ($("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").val() != '') {
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
            //                    $("#ctl00_ContentPlaceHolder1_txtBalance").val('');
            //                    $("#ctl00_ContentPlaceHolder1_txtBalance").attr("disabled", true);

            $("#ctl00_ContentPlaceHolder1_txtHExpEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtHExpEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtHExpLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtHExpLessAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtHExpGreaterAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtHExpGreaterAndEqual").attr("disabled", true);
            $("#trExpenses").remove();

        }
    });
    //Expenses
    $('#tblLoc input[name="ctl00$ContentPlaceHolder1$loc"] + label').click(function () {
        if ($(this).text() != "Any") {
            if ($(this).text() == "=") {
                if ($("#ctl00_ContentPlaceHolder1_txtLocEqual").val() == '') {
                    $("#trloc").remove();
                }
                $("#ctl00_ContentPlaceHolder1_txtLocEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").attr("disabled", true);
                $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").attr("disabled", true);
            }
            else if ($(this).text() == ">=" || $(this).text() == "&gt;") {
                $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtLocEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtLocEqual").attr("disabled", true);
                if ($("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").val() == '') {
                    $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").attr("disabled", true);
                }
                else {
                    $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").attr("disabled", false);
                }
            }
            else if ($(this).text() == "<=" || $(this).text() == "&lt;") {
                $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtLocEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtLocEqual").attr("disabled", true);
                if ($("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").val() == '') {
                    $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").attr("disabled", true);
                }
                else {
                    $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").attr("disabled", false);
                }
            }
        }
        else {
            $("#ctl00_ContentPlaceHolder1_txtLocEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtLocEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").attr("disabled", true);
            $("#trloc").remove();

        }
    });
    debugger;
    $('#tblEquip input[name="ctl00$ContentPlaceHolder1$equip"] + label').click(function () {
        if ($(this).text() != "Any") {
            if ($(this).text() == "=") {
                if ($("#ctl00_ContentPlaceHolder1_txtEquipEqual").val() == '') {
                    $("#trequip").remove();
                }
                $("#ctl00_ContentPlaceHolder1_txtEquipEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").attr("disabled", true);
                $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").attr("disabled", true);
            }
            else if ($(this).text() == ">=" || $(this).text() == "&gt;") {
                $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtEquipEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtEquipEqual").attr("disabled", true);
                if ($("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").val() == '') {
                    $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").attr("disabled", true);
                }
                else {
                    $("#ctl00_ContentPlaceHolder1_txtequipLessAndEqual").attr("disabled", false);
                }
            }
            else if ($(this).text() == "<=" || $(this).text() == "&lt;") {
                $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtEquipEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtEquipEqual").attr("disabled", true);
                if ($("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").val() == '') {
                    $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").attr("disabled", true);
                }
                else {
                    $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").attr("disabled", false);
                }
            }
        }
        else {

            $("#ctl00_ContentPlaceHolder1_txtEquipEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").attr("disabled", true);
            $("#trequip").remove();

        }
    });

    //change by Yashasvi Jadav
    $('#tblEquipmentCounts input[name="ctl00$ContentPlaceHolder1$equipmentcounts"] + label').click(function () {
        if ($(this).text() != "Any") {
            if ($(this).text() == "=") {
                if ($("#ctl00_ContentPlaceHolder1_txtEquipmentCountsEqual").val() == '') {
                    $("#trEquipmentCounts").remove();
                }
                $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsGreatAndEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsGreatAndEqual").attr("disabled", true);
                $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsLessAndEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsLessAndEqual").attr("disabled", true);
            }
            else if ($(this).text() == ">=" || $(this).text() == "&gt;") {
                $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsGreatAndEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsEqual").attr("disabled", true);
                if ($("#ctl00_ContentPlaceHolder1_txtEquipmentCountsLessAndEqual").val() == '') {
                    $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsLessAndEqual").attr("disabled", true);
                }
                else {
                    $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsLessAndEqual").attr("disabled", false);
                }
            }
            else if ($(this).text() == "<=" || $(this).text() == "&lt;") {
                $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsLessAndEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsEqual").attr("disabled", true);
                if ($("#ctl00_ContentPlaceHolder1_txtEquipmentCountsGreatAndEqual").val() == '') {
                    $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsGreatAndEqual").attr("disabled", true);
                }
                else {
                    $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsGreatAndEqual").attr("disabled", false);
                }
            }
        }
        else {
            $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsLessAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsGreatAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsGreatAndEqual").attr("disabled", true);
            $("#trEquipmentCounts").remove();

        }
    });


    $('#tblOpenCalls input[name="ctl00$ContentPlaceHolder1$oc"] + label').click(function () {
        if ($(this).text() != "Any") {
            if ($(this).text() == "=") {
                if ($("#ctl00_ContentPlaceHolder1_txtOCEqual").val() == '') {
                    $("#tropencall").remove();
                }
                $("#ctl00_ContentPlaceHolder1_txtOCEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").attr("disabled", true);
                $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").attr("disabled", true);
            }
            else if ($(this).text() == ">=" || $(this).text() == "&gt;") {
                $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtOCEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtOCEqual").attr("disabled", true);
                if ($("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").val() == '') {
                    $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").attr("disabled", true);
                }
                else {
                    $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").attr("disabled", false);
                }
            }
            else if ($(this).text() == "<=" || $(this).text() == "&lt;") {
                $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtOCEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtOCEqual").attr("disabled", true);
                if ($("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").val() == '') {
                    $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").attr("disabled", true);
                }
                else {
                    $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").attr("disabled", false);
                }
            }
        }
        else {

            $("#ctl00_ContentPlaceHolder1_txtOCEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtOCEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").attr("disabled", true);
            $("#tropencall").remove();

        }
    });

    $('#tblEquipmentPrice input[name="ctl00$ContentPlaceHolder1$ep"] + label').click(function () {
        if ($(this).text() != "Any") {
            if ($(this).text() == "=") {
                if ($("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").val() == '') {
                    $("#trEquipmentPrice").remove();
                }
                $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").attr("disabled", true);
                $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").attr("disabled", true);
            }
            else if ($(this).text() == ">=" || $(this).text() == "&gt;") {
                $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").attr("disabled", true);
                if ($("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").val() == '') {
                    $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").attr("disabled", true);
                }
                else {
                    $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").attr("disabled", false);
                }
            }
            else if ($(this).text() == "<=" || $(this).text() == "&lt;") {
                $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").attr("disabled", false);
                $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").val('');
                $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").attr("disabled", true);
                if ($("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").val() == '') {
                    $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").attr("disabled", true);
                }
                else {
                    $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").attr("disabled", false);
                }
            }
        }
        else {

            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").val('');
            $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").attr("disabled", true);
            $("#trEquipmentPrice").remove();
        }
    });

    $('#ctl00_ContentPlaceHolder1_drpType input[type="checkbox"]').click(function () {
        var filterValue = '';
        $('#ctl00_ContentPlaceHolder1_drpType input[type=checkbox]:checked + label').each(function () {
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

    $('#ctl00_ContentPlaceHolder1_drpName input[type="checkbox"]').click(function () {
        var filterValue = '';
        $('#ctl00_ContentPlaceHolder1_drpName input[type=checkbox]:checked + label').each(function () {
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
    $('#ctl00_ContentPlaceHolder1_drpRoute input[type="checkbox"]').click(function () {
        var filterValue = '';
        $('#ctl00_ContentPlaceHolder1_drpRoute input[type="checkbox"]:checked + label').each(function () {
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

    $('#ctl00_ContentPlaceHolder1_drpLocationSTax input[type="checkbox"]').click(function () {
        var filterValue = '';
        $('#ctl00_ContentPlaceHolder1_drpLocationSTax input[type="checkbox"]:checked + label').each(function () {
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

    $('#ctl00_ContentPlaceHolder1_drpDefaultSalesPerson input[type="checkbox"]').click(function () {
        var filterValue = '';
        $('#ctl00_ContentPlaceHolder1_drpDefaultSalesPerson input[type="checkbox"]:checked + label').each(function () {
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

    $('#ctl00_ContentPlaceHolder1_drpEquipmentState input[type="checkbox"]').click(function () {
        var filterValue = '';
        $('#ctl00_ContentPlaceHolder1_drpEquipmentState input[type="checkbox"]:checked + label').each(function () {
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

    $('#ctl00_ContentPlaceHolder1_drpBuldingType input[type="checkbox"]').click(function () {
        var filterValue = '';
        $('#ctl00_ContentPlaceHolder1_drpBuldingType input[type="checkbox"]:checked + label').each(function () {
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

    $('#ctl00_ContentPlaceHolder1_drpCity input[type="checkbox"]').click(function () {

        var filterValue = '';
        $('#ctl00_ContentPlaceHolder1_drpCity input[type=checkbox]:checked + label').each(function () {
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

    $('#ctl00_ContentPlaceHolder1_drpAddress input[type="checkbox"]').click(function () {
        var filterValue = '';
        $('#ctl00_ContentPlaceHolder1_drpAddress input[type=checkbox]:checked + label').each(function () {
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

    $('#ctl00_ContentPlaceHolder1_ddlState input[type="checkbox"]').click(function () {
        var filterValue = '';
        $('#ctl00_ContentPlaceHolder1_ddlState input[type=checkbox]:checked + label').each(function () {
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

    $('#ctl00_ContentPlaceHolder1_drpDateCreated input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#ctl00_ContentPlaceHolder1_drpDateCreated", "Date Created");
    });

    $('#ctl00_ContentPlaceHolder2_drpTotalOnOrder input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#ctl00_ContentPlaceHolder1_drpTotalOnOrder", "Total On Order");
    });

    $('#ctl00_ContentPlaceHolder3_drpTotal input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#ctl00_ContentPlaceHolder1_drpTotal", "Total");
    });

    $('#ctl00_ContentPlaceHolder1_drpManualInvoice input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#ctl00_ContentPlaceHolder1_drpManualInvoice", "Manual Invoice");
    });

    $('#ctl00_ContentPlaceHolder1_drpCustomerName input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#ctl00_ContentPlaceHolder1_drpCustomerName", "Customer Name");
    });

    $('#ctl00_ContentPlaceHolder1_drpType input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#ctl00_ContentPlaceHolder1_drpType", "Type");
    });

    $('#ctl00_ContentPlaceHolder1_drpCustomer input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#ctl00_ContentPlaceHolder1_drpCustomer", "Customer");
    });

    $('#ctl00_ContentPlaceHolder1_drpLocation input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#ctl00_ContentPlaceHolder1_drpLocation", "Location");
    });

    //Rahil
    $('#ctl00_ContentPlaceHolder1_drpDescription input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#ctl00_ContentPlaceHolder1_drpDescription", "Description");
    });

    $('#ctl00_ContentPlaceHolder1_drpAmountDue input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#ctl00_ContentPlaceHolder1_drpAmountDue", "Amount Due");
    });

    $('#ctl00_ContentPlaceHolder1_drpProject input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#ctl00_ContentPlaceHolder1_drpProject", "Project");
    });

    $('#ctl00_ContentPlaceHolder1_drpBatch input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#ctl00_ContentPlaceHolder1_drpBatch", "Batch");
    });

    $('#ctl00_ContentPlaceHolder1_drpDueDate input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#ctl00_ContentPlaceHolder1_drpDueDate", "Due Date");
    });

    $('#ctl00_ContentPlaceHolder1_drpCustom10 input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#ctl00_ContentPlaceHolder1_drpCustom10", "Custom10");
    });

    $('#ctl00_ContentPlaceHolder1_drpPO input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#ctl00_ContentPlaceHolder1_drpPO", "PO");
    });

    $('#ctl00_ContentPlaceHolder1_drpTotalBilled input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#ctl00_ContentPlaceHolder1_drpTotalBilled", "Total Billed");
    });

    $('#ctl00_ContentPlaceHolder1_drpSalesTax input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#ctl00_ContentPlaceHolder1_drpSalesTax", "Sales Tax");
    });

    $('#ctl00_ContentPlaceHolder1_drpPreTaxAmount input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#ctl00_ContentPlaceHolder1_drpPreTaxAmount", "Pre Tax Amount");
    });

    $('#ctl00_ContentPlaceHolder1_drpRemit input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#ctl00_ContentPlaceHolder1_drpRemit", "Remit");
    });

    //Rahil
    $('#ctl00_ContentPlaceHolder1_drpHours input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#ctl00_ContentPlaceHolder1_drpHours", "Hours");
    });

    $('#ctl00_ContentPlaceHolder1_drpLocationAddress input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#ctl00_ContentPlaceHolder1_drpLocationAddress", "LocationAddress");
    });

    $('#ctl00_ContentPlaceHolder1_drpLocationCity input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#ctl00_ContentPlaceHolder1_drpLocationCity", "LocationCity");
    });

    $('#ctl00_ContentPlaceHolder1_drpLocationState input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#ctl00_ContentPlaceHolder1_drpLocationState", "LocationState");
    });

    $('#ctl00_ContentPlaceHolder1_drpLocationZip input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#ctl00_ContentPlaceHolder1_drpLocationZip", "LocationZip");
    });

    $('#ctl00_ContentPlaceHolder1_drpLocationType input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#ctl00_ContentPlaceHolder1_drpLocationType", "LocationType");
    });

    $('#ctl00_ContentPlaceHolder1_drpEquipmentName input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#ctl00_ContentPlaceHolder1_drpEquipmentName", "EquipmentName");
    });

    $('#ctl00_ContentPlaceHolder1_drpManuf input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#ctl00_ContentPlaceHolder1_drpManuf", "Manuf");
    });

    $('#ctl00_ContentPlaceHolder1_drpEquipmentType input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#ctl00_ContentPlaceHolder1_drpEquipmentType", "EquipmentType");
    });

    $('#ctl00_ContentPlaceHolder1_drpServiceType input[type="checkbox"]').click(function () {
        SetFiltersChoicesFromDropDown("#ctl00_ContentPlaceHolder1_drpServiceType", "ServiceType");
    });


    $("#btnCustomizeReport").click(function () {
        debugger;
        $("#ctl00_ContentPlaceHolder1_chkColumnList INPUT[type='checkbox']").attr('checked', false);
        // var reportValue = $("#ctl00_ContentPlaceHolder1_drpReports option:selected").text();
        $("#ctl00_ContentPlaceHolder1_hdnReportAction").val('Update');
        var reportName = $("#ctl00_ContentPlaceHolder1_hdnCustomizeReportName").val();
        $('#spnModelTitle').html('Modify Report : ' + reportName);
        $("#ctl00_ContentPlaceHolder1_txtReportName").val(reportName);
        var columnList = $("#ctl00_ContentPlaceHolder1_hdnColumnList").val();

        var splitedColList = columnList.split(",");
        for (i = 0; i <= splitedColList.length; i++) {
            $('#ctl00_ContentPlaceHolder1_chkColumnList input[type=checkbox]').each(function () {
                var lblValue = $(this).parent().find('label').html();
                if (lblValue == splitedColList[i]) {
                    $(this).attr('checked', true);
                }
            });
        }

        $("#ctl00_ContentPlaceHolder1_drpSortBy option").remove();
        $("#ctl00_ContentPlaceHolder1_lstColumnSort option").remove();

        $.each(splitedColList, function (key, value) {

            $('#ctl00_ContentPlaceHolder1_drpSortBy')
         .append($("<option></option>")
         .attr("value", value)
         .text(value));


            $('#ctl00_ContentPlaceHolder1_lstColumnSort')
                                            .append($("<option></option>")
                                 .attr("value", value)
                                 .text(value));

        });

        /* if ($('#tblResize tr').length > 0) {
             $('#ctl00_ContentPlaceHolder1_lstColumnSort option').remove();
             $('#tblResize tr th').each(function() {
                 $('#ctl00_ContentPlaceHolder1_lstColumnSort')
                                     .append($("<option></option>")
                          .attr("value", $(this).html())
                          .text($(this).html()));
             });
         }*/


        debugger;
        $('#ctl00_ContentPlaceHolder1_drpSortBy').val($("#ctl00_ContentPlaceHolder1_hdnDrpSortBy").val());
        if (filters.length == 0) {
            $("#tblFilterChoices tbody").empty();
            $("#ctl00_ContentPlaceHolder1_lstFilter").val('Name');
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
                            $("#ctl00_ContentPlaceHolder1_ddlStateReference option[value= " + actualText + "]").attr("selected", "selected")
                            getText += "'" + $("#ctl00_ContentPlaceHolder1_ddlStateReference option:selected").text() + "'" + " | ";

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
                $("#ctl00_ContentPlaceHolder1_lstFilter").val(filterColumns);
            });
        }
    });

    $('#ctl00_ContentPlaceHolder1_chkColumnList').change(function () {
        $("#ctl00_ContentPlaceHolder1_drpSortBy option").remove();
        $('#ctl00_ContentPlaceHolder1_lstColumnSort option').remove();
        //  var lstColumnValues = '';
        $('#ctl00_ContentPlaceHolder1_chkColumnList input[type=checkbox]').each(function () {
            if ($(this).is(":checked")) {
                $('#ctl00_ContentPlaceHolder1_drpSortBy').append($("<option></option>").attr("value", $(this).parent().find('label').html()).text($(this).parent().find('label').html()));

                $('#ctl00_ContentPlaceHolder1_lstColumnSort').append($("<option></option>").attr("value", $(this).parent().find('label').html()).text($(this).parent().find('label').html()));

            }

        });

    });


    $("#ctl00_ContentPlaceHolder1_lstFilter").change(function () {
        var filterName = $("#ctl00_ContentPlaceHolder1_lstFilter option:selected").text();
        filterName = filterName.replace(" ", "");
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
        $("#ctl00_ContentPlaceHolder1_lstFilter").val(filterName);
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

    $("#ctl00_ContentPlaceHolder1_txtZip").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtZip").val();
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

    $("#ctl00_ContentPlaceHolder1_txtInstalledOn").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtInstalledOn").val();
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

    $("#ctl00_ContentPlaceHolder1_txtPhone").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtPhone").val();
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

    $("#ctl00_ContentPlaceHolder1_txtFax").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtFax").val();
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

    $("#ctl00_ContentPlaceHolder1_txtContact").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtContact").val();
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

    $("#ctl00_ContentPlaceHolder1_txtEmail").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtEmail").val();
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

    $("#ctl00_ContentPlaceHolder1_txtCountry").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtCountry").val();
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

    $("#ctl00_ContentPlaceHolder1_txtWebsite").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtWebsite").val();
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

    $("#ctl00_ContentPlaceHolder1_txtCellular").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtCellular").val();
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

    $("#ctl00_ContentPlaceHolder1_drpCategory").change(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_drpCategory option:selected").text();
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


    $("#ctl00_ContentPlaceHolder1_txtBalEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtBalEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblBalance input[name='ctl00$ContentPlaceHolder1$Balance']:checked + label").text();
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


    //Expenses
   
    $("#ctl00_ContentPlaceHolder1_txtExpEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtExpEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblExpenses input[name='ctl00$ContentPlaceHolder1$Expenses']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("expenses")) {
                $(".expenses").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trExpenses"><td width="100px" style="height:25px;">Expenses</td><td  class="expenses" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        //                else {
        //                    $("#trBalance").remove();
        //                }
    });

    $("#ctl00_ContentPlaceHolder1_txtTOExpEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtTOExpEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblTotalOnOrder input[name='ctl00$ContentPlaceHolder1$TotalOnOrder']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("totalonorder")) {
                $(".totalonorder").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trTotalOnOrder"><td width="100px" style="height:25px;">Total On Order</td><td  class="totalonorder" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        //                else {
        //                    $("#trBalance").remove();
        //                }
    });
   
    $("#ctl00_ContentPlaceHolder1_txtLExpEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtLExpEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblLaborExpense input[name='ctl00$ContentPlaceHolder1$LaborExpense']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("laborexpense")) {
                $(".laborexpense").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
            }
            else {
                
                $('#tblFilterChoices tbody').append('<tr id="trLaborExpense"><td width="100px" style="height:25px;">Labor Expense</td><td  class="laborexpense" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        //                else {
        //                    $("#trBalance").remove();
        //                }
    });

    $("#ctl00_ContentPlaceHolder1_txtTExpEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtTExpEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblTotalExpenses input[name='ctl00$ContentPlaceHolder1$TotalExpenses']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("totalexpenses")) {
                $(".totalexpenses").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
            }
            else {

                $('#tblFilterChoices tbody').append('<tr id="trTotalExpenses"><td width="100px" style="height:25px;">Total Expenses</td><td  class="totalexpenses" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        //                else {
        //                    $("#trBalance").remove();
        //                }
    });

    $("#ctl00_ContentPlaceHolder1_txtMExpEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtMExpEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblMaterialExpense input[name='ctl00$ContentPlaceHolder1$MaterialExpense']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("materialexpense")) {
                $(".materialexpense").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
            }
            else {
                
                $('#tblFilterChoices tbody').append('<tr id="trMaterialExpense"><td width="100px" style="height:25px;">Material Expense</td><td  class="materialexpense" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        //                else {
        //                    $("#trBalance").remove();
        //                }
    });

    $("#ctl00_ContentPlaceHolder1_txtBExpEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtBExpEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblTotalBilled input[name='ctl00$ContentPlaceHolder1$TotalBilled']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("totalbilled")) {
                $(".totalbilled").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
            }
            else {

                $('#tblFilterChoices tbody').append('<tr id="trTotalBilled"><td width="100px" style="height:25px;">Total Billed</td><td  class="totalbilled" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        //                else {
        //                    $("#trBalance").remove();
        //                }
    });

    $("#ctl00_ContentPlaceHolder1_txtHExpEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtHExpEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblHours input[name='ctl00$ContentPlaceHolder1$Hours']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("hours")) {
                $(".hours").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trHours"><td width="100px" style="height:25px;">Hours</td><td  class="hours" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        //                else {
        //                    $("#trBalance").remove();
        //                }
    });
    //Expenses

    $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblBalance input[name='ctl00$ContentPlaceHolder1$Balance']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("balance") && $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").val() != '') {
                getLessValue = "<=" + $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").val();
                $(".balance").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getLessValue);
                $(".balance").html(">=" + filterValue + " and " + getLessValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("balance") && $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".balance").html(">=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trBalance"><td width="100px" style="height:25px;">Balance</td><td  class="balance" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").val() == '') {
                $("#trBalance").remove();
            }
            else {
                $(".balance").html("<=" + $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").val());
            }
        }
    });

    //Expenses
    
    $("#ctl00_ContentPlaceHolder1_txtExpGreaterAndEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtExpGreaterAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblExpenses input[name='ctl00$ContentPlaceHolder1$Expenses']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("expenses") && $("#ctl00_ContentPlaceHolder1_txtExpLessAndEqual").val() != '') {
                getLessValue = "<=" + $("#ctl00_ContentPlaceHolder1_txtExpLessAndEqual").val();
                $(".expenses").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getLessValue);
                $(".expenses").html(">=" + filterValue + " and " + getLessValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("expenses") && $("#ctl00_ContentPlaceHolder1_txtExpLessAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".expenses").html(">=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trExpenses"><td width="100px" style="height:25px;">Expenses</td><td  class="expenses" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#ctl00_ContentPlaceHolder1_txtExpLessAndEqual").val() == '') {
                $("#trExpenses").remove();
            }
            else {
                $(".expenses").html("<=" + $("#ctl00_ContentPlaceHolder1_txtExpLessAndEqual").val());
            }
        }
    });
   
    $("#ctl00_ContentPlaceHolder1_txtLExpGreaterAndEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtLExpGreaterAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblLaborExpense input[name='ctl00$ContentPlaceHolder1$LaborExpense']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("laborexpense") && $("#ctl00_ContentPlaceHolder1_txtLExpLessAndEqual").val() != '') {
                getLessValue = "<=" + $("#ctl00_ContentPlaceHolder1_txtLExpLessAndEqual").val();
                $(".laborexpense").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getLessValue);
                $(".laborexpense").html(">=" + filterValue + " and " + getLessValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("laborexpense") && $("#ctl00_ContentPlaceHolder1_txtLExpLessAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".laborexpense").html(">=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trLaborExpense"><td width="100px" style="height:25px;">Labor Expense</td><td  class="laborexpense" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#ctl00_ContentPlaceHolder1_txtLExpLessAndEqual").val() == '') {
                $("#trLaborExpense").remove();
            }
            else {
                $(".laborexpense").html("<=" + $("#ctl00_ContentPlaceHolder1_txtLExpLessAndEqual").val());
            }
        }
    });

   

    $("#ctl00_ContentPlaceHolder1_txtTOExpGreaterAndEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtTOExpGreaterAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblTotalOnOrder input[name='ctl00$ContentPlaceHolder1$TotalOnOrder']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("totalonorder") && $("#ctl00_ContentPlaceHolder1_txtTOExpLessAndEqual").val() != '') {
                getLessValue = "<=" + $("#ctl00_ContentPlaceHolder1_txtTOExpLessAndEqual").val();
                $(".totalonorder").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getLessValue);
                $(".totalonorder").html(">=" + filterValue + " and " + getLessValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("totalonorder") && $("#ctl00_ContentPlaceHolder1_txtTOExpLessAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".totalonorder").html(">=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trTotalOnOrder"><td width="100px" style="height:25px;">Total On Order</td><td  class="totalonorder" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#ctl00_ContentPlaceHolder1_txtTOExpLessAndEqual").val() == '') {
                $("#trTotalOnOrder").remove();
            }
            else {
                $(".totalonorder").html("<=" + $("#ctl00_ContentPlaceHolder1_txtTOExpLessAndEqual").val());
            }
        }
    });

    $("#ctl00_ContentPlaceHolder1_txtTExpGreaterAndEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtTExpGreaterAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblTotalExpenses input[name='ctl00$ContentPlaceHolder1$TotalExpenses']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("totalexpenses") && $("#ctl00_ContentPlaceHolder1_txtTExpLessAndEqual").val() != '') {
                getLessValue = "<=" + $("#ctl00_ContentPlaceHolder1_txtTExpLessAndEqual").val();
                $(".totalexpenses").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getLessValue);
                $(".totalexpenses").html(">=" + filterValue + " and " + getLessValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("totalexpenses") && $("#ctl00_ContentPlaceHolder1_txtTExpLessAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".totalexpenses").html(">=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trTotalExpenses"><td width="100px" style="height:25px;">Total Expenses</td><td  class="totalexpenses" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#ctl00_ContentPlaceHolder1_txtTExpLessAndEqual").val() == '') {
                $("#trTotalExpenses").remove();
            }
            else {
                $(".totalexpenses").html("<=" + $("#ctl00_ContentPlaceHolder1_txtTExpLessAndEqual").val());
            }
        }
    });

    $("#ctl00_ContentPlaceHolder1_txtMExpGreaterAndEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtMExpGreaterAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblMaterialExpense input[name='ctl00$ContentPlaceHolder1$MaterialExpense']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("materialexpense") && $("#ctl00_ContentPlaceHolder1_txtMExpLessAndEqual").val() != '') {
                getLessValue = "<=" + $("#ctl00_ContentPlaceHolder1_txtMExpLessAndEqual").val();
                $(".materialexpense").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getLessValue);
                $(".materialexpense").html(">=" + filterValue + " and " + getLessValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("materialexpense") && $("#ctl00_ContentPlaceHolder1_txtMExpLessAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".materialexpense").html(">=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trMaterialExpense"><td width="100px" style="height:25px;">Material Expense</td><td  class="materialexpense" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#ctl00_ContentPlaceHolder1_txtMExpLessAndEqual").val() == '') {
                $("#trMaterialExpense").remove();
            }
            else {
                $(".materialexpense").html("<=" + $("#ctl00_ContentPlaceHolder1_txtMExpLessAndEqual").val());
            }
        }
    });    


    $("#ctl00_ContentPlaceHolder1_txtBExpGreaterAndEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtBExpGreaterAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblTotalBilled input[name='ctl00$ContentPlaceHolder1$TotalBilled']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("totalbilled") && $("#ctl00_ContentPlaceHolder1_txtBExpLessAndEqual").val() != '') {
                getLessValue = "<=" + $("#ctl00_ContentPlaceHolder1_txtBExpLessAndEqual").val();
                $(".totalbilled").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getLessValue);
                $(".totalbilled").html(">=" + filterValue + " and " + getLessValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("totalbilled") && $("#ctl00_ContentPlaceHolder1_txtBExpLessAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".totalbilled").html(">=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trTotalBilled"><td width="100px" style="height:25px;">Total Billed</td><td  class="totalbilled" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#ctl00_ContentPlaceHolder1_txtBExpLessAndEqual").val() == '') {
                $("#trTotalBilled").remove();
            }
            else {
                $(".totalbilled").html("<=" + $("#ctl00_ContentPlaceHolder1_txtBExpLessAndEqual").val());
            }
        }
    });
    $("#ctl00_ContentPlaceHolder1_txtHExpGreaterAndEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtHExpGreaterAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblHours input[name='ctl00$ContentPlaceHolder1$Hours']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("hours") && $("#ctl00_ContentPlaceHolder1_txtHExpLessAndEqual").val() != '') {
                getLessValue = "<=" + $("#ctl00_ContentPlaceHolder1_txtHExpLessAndEqual").val();
                $(".hours").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getLessValue);
                $(".hours").html(">=" + filterValue + " and " + getLessValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("hours") && $("#ctl00_ContentPlaceHolder1_txtHExpLessAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".hours").html(">=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trHours"><td width="100px" style="height:25px;">Hours</td><td  class="hours" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#ctl00_ContentPlaceHolder1_txtHExpLessAndEqual").val() == '') {
                $("#trHours").remove();
            }
            else {
                $(".hours").html("<=" + $("#ctl00_ContentPlaceHolder1_txtHExpLessAndEqual").val());
            }
        }
    });
    //Expenses
    $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtBalLessAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblBalance input[name='ctl00$ContentPlaceHolder1$Balance']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("balance") && $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").val() != '') {
                getGreaterValue = ">=" + $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").val()
                $(".balance").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getGreaterValue);
                $(".balance").html(getGreaterValue + " and " + "<=" + filterValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("balance") && $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".balance").html("<=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trBalance"><td width="100px" style="height:25px;">Balance</td><td  class="balance" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").val() == '') {
                $("#trBalance").remove();
            }
            else {
                $(".balance").html(">=" + $("#ctl00_ContentPlaceHolder1_txtBalGreaterAndEqual").val());
            }
        }
    });

    //Expenses
    
    $("#ctl00_ContentPlaceHolder1_txtExpLessAndEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtExpLessAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblExpenses input[name='ctl00$ContentPlaceHolder1$Expenses']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("expenses") && $("#ctl00_ContentPlaceHolder1_txtExpGreaterAndEqual").val() != '') {
                getGreaterValue = ">=" + $("#ctl00_ContentPlaceHolder1_txtExpGreaterAndEqual").val()
                $(".expenses").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getGreaterValue);
                $(".expenses").html(getGreaterValue + " and " + "<=" + filterValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("expenses") && $("#ctl00_ContentPlaceHolder1_txtExpGreaterAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".expenses").html("<=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trExpenses"><td width="100px" style="height:25px;">Expenses</td><td  class="expenses" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#ctl00_ContentPlaceHolder1_txtExpGreaterAndEqual").val() == '') {
                $("#trExpenses").remove();
            }
            else {
                $(".expenses").html(">=" + $("#ctl00_ContentPlaceHolder1_txtExpGreaterAndEqual").val());
            }
        }
    });
    
    $("#ctl00_ContentPlaceHolder1_txtLExpLessAndEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtLExpLessAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblLaborExpense input[name='ctl00$ContentPlaceHolder1$LaborExpense']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("laborexpense") && $("#ctl00_ContentPlaceHolder1_txtLExpGreaterAndEqual").val() != '') {
                getGreaterValue = ">=" + $("#ctl00_ContentPlaceHolder1_txtLExpGreaterAndEqual").val()
                $(".laborexpense").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getGreaterValue);
                $(".laborexpense").html(getGreaterValue + " and " + "<=" + filterValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("laborexpense") && $("#ctl00_ContentPlaceHolder1_txtLExpGreaterAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".laborexpense").html("<=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trLaborExpense"><td width="100px" style="height:25px;">Labor Expense</td><td  class="laborexpense" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#ctl00_ContentPlaceHolder1_txtLExpGreaterAndEqual").val() == '') {
                $("#trLaborExpense").remove();
            }
            else {
                $(".laborexpense").html(">=" + $("#ctl00_ContentPlaceHolder1_txtLExpGreaterAndEqual").val());
            }
        }
    });

    $("#ctl00_ContentPlaceHolder1_txtTExpLessAndEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtTExpLessAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblTotalExpenses input[name='ctl00$ContentPlaceHolder1$TotalExpenses']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("totalexpenses") && $("#ctl00_ContentPlaceHolder1_txtTExpGreaterAndEqual").val() != '') {
                getGreaterValue = ">=" + $("#ctl00_ContentPlaceHolder1_txtTExpGreaterAndEqual").val()
                $(".totalexpenses").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getGreaterValue);
                $(".totalexpenses").html(getGreaterValue + " and " + "<=" + filterValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("totalexpenses") && $("#ctl00_ContentPlaceHolder1_txtTExpGreaterAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".totalexpenses").html("<=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trTotalExpenses"><td width="100px" style="height:25px;">Total Expenses</td><td  class="totalexpenses" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#ctl00_ContentPlaceHolder1_txtTExpGreaterAndEqual").val() == '') {
                $("#trTotalExpenses").remove();
            }
            else {
                $(".totalexpenses").html(">=" + $("#ctl00_ContentPlaceHolder1_txtTExpGreaterAndEqual").val());
            }
        }
    });

    $("#ctl00_ContentPlaceHolder1_txtTOExpLessAndEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtTOExpLessAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblTotalOnOrder input[name='ctl00$ContentPlaceHolder1$TotalOnOrder']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("totalonorder") && $("#ctl00_ContentPlaceHolder1_txtTOExpGreaterAndEqual").val() != '') {
                getGreaterValue = ">=" + $("#ctl00_ContentPlaceHolder1_txtTOExpGreaterAndEqual").val()
                $(".totalonorder").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getGreaterValue);
                $(".totalonorder").html(getGreaterValue + " and " + "<=" + filterValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("totalonorder") && $("#ctl00_ContentPlaceHolder1_txtTOExpGreaterAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".totalonorder").html("<=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trTotalOnOrder"><td width="100px" style="height:25px;">Total On Order</td><td  class="totalonorder" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#ctl00_ContentPlaceHolder1_txtTOExpGreaterAndEqual").val() == '') {
                $("#trTotalOnOrder").remove();
            }
            else {
                $(".totalonorder").html(">=" + $("#ctl00_ContentPlaceHolder1_txtTOExpGreaterAndEqual").val());
            }
        }
    });

    $("#ctl00_ContentPlaceHolder1_txtMExpLessAndEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtMExpLessAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblMaterialExpense input[name='ctl00$ContentPlaceHolder1$MaterialExpense']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("materialexpense") && $("#ctl00_ContentPlaceHolder1_txtMExpGreaterAndEqual").val() != '') {
                getGreaterValue = ">=" + $("#ctl00_ContentPlaceHolder1_txtMExpGreaterAndEqual").val()
                $(".materialexpense").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getGreaterValue);
                $(".materialexpense").html(getGreaterValue + " and " + "<=" + filterValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("materialexpense") && $("#ctl00_ContentPlaceHolder1_txtMExpGreaterAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".materialexpense").html("<=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trMaterialExpense"><td width="100px" style="height:25px;">Material Expense</td><td  class="materialexpense" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#ctl00_ContentPlaceHolder1_txtMExpGreaterAndEqual").val() == '') {
                $("#trMaterialExpense").remove();
            }
            else {
                $(".materialexpense").html(">=" + $("#ctl00_ContentPlaceHolder1_txtMExpGreaterAndEqual").val());
            }
        }
    });

  

    $("#ctl00_ContentPlaceHolder1_txtHExpLessAndEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtHExpLessAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblHours input[name='ctl00$ContentPlaceHolder1$Hours']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("hours") && $("#ctl00_ContentPlaceHolder1_txtHExpGreaterAndEqual").val() != '') {
                getGreaterValue = ">=" + $("#ctl00_ContentPlaceHolder1_txtHExpGreaterAndEqual").val()
                $(".hours").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getGreaterValue);
                $(".hours").html(getGreaterValue + " and " + "<=" + filterValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("hours") && $("#ctl00_ContentPlaceHolder1_txtHExpGreaterAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".hours").html("<=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trHours"><td width="100px" style="height:25px;">Hours</td><td  class="hours" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#ctl00_ContentPlaceHolder1_txtHExpGreaterAndEqual").val() == '') {
                $("#trExpenses").remove();
            }
            else {
                $(".hours").html(">=" + $("#ctl00_ContentPlaceHolder1_txtHExpGreaterAndEqual").val());
            }
        }
    });
    //Expenses

    $("#ctl00_ContentPlaceHolder1_txtBExpLessAndEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtBExpLessAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblTotalBilled input[name='ctl00$ContentPlaceHolder1$TotalBilled']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("totalbilled") && $("#ctl00_ContentPlaceHolder1_txtBExpGreaterAndEqual").val() != '') {
                getGreaterValue = ">=" + $("#ctl00_ContentPlaceHolder1_txtBExpGreaterAndEqual").val()
                $(".totalbilled").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getGreaterValue);
                $(".totalbilled").html(getGreaterValue + " and " + "<=" + filterValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("totalbilled") && $("#ctl00_ContentPlaceHolder1_txtBExpGreaterAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".totalbilled").html("<=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trTotalBilled"><td width="100px" style="height:25px;">Total Billed</td><td  class="TotalBilled" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#ctl00_ContentPlaceHolder1_txtBExpGreaterAndEqual").val() == '') {
                $("#trTotalBilled").remove();
            }
            else {
                $(".totalbilled").html(">=" + $("#ctl00_ContentPlaceHolder1_txtBExpGreaterAndEqual").val());
            }
        }
    });

    $("#ctl00_ContentPlaceHolder1_drpStatus").change(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_drpStatus option:selected").text();
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


    $("#ctl00_ContentPlaceHolder1_txtLocEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtLocEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblLoc input[name='ctl00$ContentPlaceHolder1$loc']:checked + label").text();
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

  

    $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblLoc input[name='ctl00$ContentPlaceHolder1$loc']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("loc") && $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").val() != '') {
                getGreaterValue = ">=" + $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").val()
                $(".loc").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getGreaterValue);
                $(".loc").html(getGreaterValue + " and " + "<=" + filterValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("loc") && $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".loc").html("<=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trloc"><td width="100px" style="height:25px;">loc</td><td  class="loc" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").val() == '') {
                $("#trloc").remove();
            }
            else {
                $(".loc").html(">=" + $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").val());
            }
        }
    });


    $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtLocGreatAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblLoc input[name='ctl00$ContentPlaceHolder1$loc']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("loc") && $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").val() != '') {
                getLessValue = "<=" + $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").val();
                $(".loc").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getLessValue);
                $(".loc").html(">=" + filterValue + " and " + getLessValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("loc") && $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".loc").html(">=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trloc"><td width="100px" style="height:25px;">loc</td><td  class="loc" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").val() == '') {
                $("#trloc").remove();
            }
            else {
                $(".loc").html("<=" + $("#ctl00_ContentPlaceHolder1_txtLocLessAndEqual").val());
            }
        }
    });

    $("#ctl00_ContentPlaceHolder1_txtEquipEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtEquipEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblEquip input[name='ctl00$ContentPlaceHolder1$equip']:checked + label").text();
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


    $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblEquip input[name='ctl00$ContentPlaceHolder1$equip']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("equip") && $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").val() != '') {
                getLessValue = "<=" + $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").val();
                $(".equip").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getLessValue);
                $(".equip").html(">=" + filterValue + " and " + getLessValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("equip") && $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".equip").html(">=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trequip"><td width="100px" style="height:25px;">equip</td><td  class="equip" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").val() == '') {
                $("#trequip").remove();
            }
            else {
                $(".equip").html("<=" + $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").val());
            }
        }
    });

    $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtEquipLessAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblEquip input[name='ctl00$ContentPlaceHolder1$equip']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("equip") && $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").val() != '') {
                getGreaterValue = ">=" + $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").val()
                $(".equip").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getGreaterValue);
                $(".equip").html(getGreaterValue + " and " + "<=" + filterValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("equip") && $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".equip").html("<=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trequip"><td width="100px" style="height:25px;">equip</td><td  class="equip" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").val() == '') {
                $("#trequip").remove();
            }
            else {
                $(".equip").html(">=" + $("#ctl00_ContentPlaceHolder1_txtEquipGreatAndEqual").val());
            }
        }
    });


    //Added by Yashasvi Jadav
    $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblEquipmentCounts input[name='ctl00$ContentPlaceHolder1$equipmentcounts']:checked + label").text();
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


    $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsGreatAndEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsGreatAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblEquipmentCounts input[name='ctl00$ContentPlaceHolder1$equipmentcounts']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("equipmentcounts") && $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsLessAndEqual").val() != '') {
                getLessValue = "<=" + $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsLessAndEqual").val();
                $(".equipmentcounts").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getLessValue);
                $(".equipmentcounts").html(">=" + filterValue + " and " + getLessValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("equipmentcounts") && $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsLessAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".equipmentcounts").html(">=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trEquipmentCounts"><td width="100px" style="height:25px;">EquipmentCounts</td><td  class="equipmentcounts" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#ctl00_ContentPlaceHolder1_txtEquipmentCountsLessAndEqual").val() == '') {
                $("#trEquipmentCounts").remove();
            }
            else {
                $(".equipmentcounts").html("<=" + $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsLessAndEqual").val());
            }
        }
    });

    $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsLessAndEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsLessAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblEquipmentCounts input[name='ctl00$ContentPlaceHolder1$equipmentCounts']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("equipmentcounts") && $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsGreatAndEqual").val() != '') {
                getGreaterValue = ">=" + $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsGreatAndEqual").val()
                $(".equipmentcounts").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getGreaterValue);
                $(".equipmentcounts").html(getGreaterValue + " and " + "<=" + filterValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("equip") && $("#ctl00_ContentPlaceHolder1_txtEquipmentcountsGreatAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".equipmentcounts").html("<=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trEquipmentCounts"><td width="100px" style="height:25px;">EquipmentCounts</td><td  class="equip" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#ctl00_ContentPlaceHolder1_txtEquipmentCountsGreatAndEqual").val() == '') {
                $("#trequip").remove();
            }
            else {
                $(".equipmentcounts").html(">=" + $("#ctl00_ContentPlaceHolder1_txtEquipmentCountsGreatAndEqual").val());
            }
        }
    });



    $("#ctl00_ContentPlaceHolder1_txtOCEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtOCEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblOpenCalls input[name='ctl00$ContentPlaceHolder1$oc']:checked + label").text();
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




    $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblOpenCalls input[name='ctl00$ContentPlaceHolder1$oc']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("opencall") && $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").val() != '') {
                getLessValue = "<=" + $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").val();
                $(".opencall").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getLessValue);
                $(".opencall").html(">=" + filterValue + " and " + getLessValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("opencall") && $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".opencall").html(">=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="tropencall"><td width="100px" style="height:25px;">opencall</td><td  class="opencall" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").val() == '') {
                $("#tropencall").remove();
            }
            else {
                $(".opencall").html("<=" + $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").val());
            }
        }
    });

    $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").blur(function () {
        debugger;
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtOCLessAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblOpenCalls input[name='ctl00$ContentPlaceHolder1$oc']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("opencall") && $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").val() != '') {
                getGreaterValue = ">=" + $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").val()
                $(".opencall").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getGreaterValue);
                $(".opencall").html(getGreaterValue + " and " + "<=" + filterValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("opencall") && $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".opencall").html("<=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="tropencall"><td width="100px" style="height:25px;">opencall</td><td  class="opencall" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").val() == '') {
                $("#tropencall").remove();
            }
            else {
                $(".opencall").html(">=" + $("#ctl00_ContentPlaceHolder1_txtOCGreatAndEqual").val());
            }
        }
    });
    

    $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblEquipmentPrice input[name='ctl00$ContentPlaceHolder1$ep']:checked + label").text();
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
    

    $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblEquipmentPrice input[name='ctl00$ContentPlaceHolder1$ep']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("equipmentprice") && $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").val() != '') {
                getLessValue = "<=" + $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").val();
                $(".equipmentprice").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getLessValue);
                $(".equipmentprice").html(">=" + filterValue + " and " + getLessValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("equipmentprice") && $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".equipmentprice").html(">=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trEquipmentPrice"><td width="100px" style="height:25px;">EquipmentPrice</td><td  class="equipmentprice" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").val() == '') {
                $("#trEquipmentPrice").remove();
            }
            else {
                $(".equipmentprice").html("<=" + $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").val());
            }
        }
    });

    $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").blur(function () {
        var filterValue = $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceLessAndEqual").val();
        if (filterValue != '') {
            var getOperator = $("#tblEquipmentPrice input[name='ctl00$ContentPlaceHolder1$ep']:checked + label").text();
            if ($('#tblFilterChoices tr td').hasClass("equipmentprice") && $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").val() != '') {
                getGreaterValue = ">=" + $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").val()
                $(".equipmentprice").html('');
                //$(".balance").html((((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue) + " and " + getGreaterValue);
                $(".equipmentprice").html(getGreaterValue + " and " + "<=" + filterValue);
            }
            else if ($('#tblFilterChoices tr td').hasClass("equipmentprice") && $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").val() == '') {
                //$(".balance").html(((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue);
                $(".equipmentprice").html("<=" + filterValue);
            }
            else {
                $('#tblFilterChoices tbody').append('<tr id="trEquipmentPrice"><td width="100px" style="height:25px;">EquipmentPrice</td><td  class="equipmentprice" width="220px"  style="height:25px;">' + ((getOperator == "Any") ? "" : getOperator) + ' ' + filterValue + '</td></tr>');
            }
        }
        else {
            if ($("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").val() == '') {
                $("#trEquipmentPrice").remove();
            }
            else {
                $(".equipmentprice").html(">=" + $("#ctl00_ContentPlaceHolder1_txtEquipmentPriceGreatAndEqual").val());
            }
        }
    });

    //    $("#ctl00_ContentPlaceHolder1_txtLocationZip").blur(function() {
    //        SetFiltersChoicesFromTextBox("#ctl00_ContentPlaceHolder1_txtLocationZip", "LocationZip");
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
    $("#ctl00_ContentPlaceHolder1_chkCompanyName").click(function () {
        if ($(this).is(":checked")) {
            $("#ctl00_ContentPlaceHolder1_txtCompanyName").attr("disabled", false);
        }
        else {
            $("#ctl00_ContentPlaceHolder1_txtCompanyName").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtCompanyName").val('');
        }
    });

    $("#ctl00_ContentPlaceHolder1_chkReportTitle").click(function () {
        if ($(this).is(":checked")) {
            $("#ctl00_ContentPlaceHolder1_txtReportTitle").attr("disabled", false);

        }
        else {
            $("#ctl00_ContentPlaceHolder1_txtReportTitle").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtReportTitle").val('');
        }
    });


    $("#ctl00_ContentPlaceHolder1_chkSubtitle").click(function () {
        if ($(this).is(":checked")) {
            $("#ctl00_ContentPlaceHolder1_txtSubtitle").attr("disabled", false);

        }
        else {
            $("#ctl00_ContentPlaceHolder1_txtSubtitle").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtSubtitle").val('');
        }
    });

    $("#ctl00_ContentPlaceHolder1_chkDatePrepared").click(function () {
        if ($(this).is(":checked")) {
            $("#ctl00_ContentPlaceHolder1_drpDatePrepared").attr("disabled", false);
        }
        else {
            $("#ctl00_ContentPlaceHolder1_drpDatePrepared").attr("disabled", true);
        }
    });


    $("#ctl00_ContentPlaceHolder1_chkPageNumber").click(function () {
        if ($(this).is(":checked")) {
            $("#ctl00_ContentPlaceHolder1_drpPageNumber").attr("disabled", false);
        }
        else {
            $("#ctl00_ContentPlaceHolder1_drpPageNumber").attr("disabled", true);
        }
    });

    $("#ctl00_ContentPlaceHolder1_chkExtraFootLine").click(function () {
        if ($(this).is(":checked")) {
            $("#ctl00_ContentPlaceHolder1_txtExtraFooterLine").attr("disabled", false);

        }
        else {
            $("#ctl00_ContentPlaceHolder1_txtExtraFooterLine").attr("disabled", true);
            $("#ctl00_ContentPlaceHolder1_txtExtraFooterLine").val('');
        }
    });

});

