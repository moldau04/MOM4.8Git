function setFinancialPermission() {
    var param = { permissionType: 'FinanceStatement' };

        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "KPI/KPIWebService.asmx/getPermissions",
            data: JSON.stringify(param),
            dataType: "json",
            success: function (data) {
                if (data.d) {
                    $('#divActualvsBudgetedRevenueKPI').show();
                }
                else {
                    $('#divActualvsBudgetedRevenueKPI').hide();
                }
            },
            error: function (result) {
                //alert(result.responseText);
            }
        })
    }

function setSalesPermission() {
    var param = { permissionType: 'Sales' };
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "KPI/KPIWebService.asmx/getPermissions",
        data: JSON.stringify(param),
        dataType: "json",
        success: function (data) {
            if (data.d) {
                $('#divAvgEstimateConversionRate').show();
            }
            else {
                //$('#divAvgEstimateConversionRate').hide();
            }
        },
        error: function (result) {
            //alert(result.responseText);
        }
    })
}