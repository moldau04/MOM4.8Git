<%@ Control Language="C#" AutoEventWireup="true" Inherits="KPI_ActualvsBudgetedRevenue" Codebehind="ActualvsBudgetedRevenue.ascx.cs" %>

<div class="container">
    <div class="row">
        <div class="col s6 m6 l2">
            <label>Select Budget:</label>
        </div>
        <div class="col s6 m6 l2" style="height: 80px;margin-top:-60px;">
            <select id="ddlBudgetList"></select>
        </div>  

    </div>
    <div class="row">
        <div id="actualvsBudgetedRevenue" style="display: block;height:350px">
        </div>
    </div>

    <script>


        $('#ddlBudgetList').on('change', function () {
            var param = { selectedBudget: this.value };
            if (param.selectedBudget == undefined) {
                param.selectedBudget = '';
            }
            createActualvsBudgetedRevenue(param);
        });

        function createActualvsBudgetedRevenue(param) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "KPI/KPIWebService.asmx/GetActualvsBudgetedRevenue",
                data: JSON.stringify(param),
                dataType: "json",
                success: function (data) {
                    createLineChartLines($("#actualvsBudgetedRevenue"), data, "Actual vs Budgeted Revenue");
                },
                error: function (result) {
                    //alert(result.responseText);
                }
            })
        }

        function getBudgetNames() {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "KPI/KPIWebService.asmx/GetBudgetNames",
                data: {},
                dataType: "json",
                success: function (data) {
                    var dropdown = $('#ddlBudgetList');
                    var option = '';
                    for (var i = 0; i < data.d.length; i++) {
                        option += '<option value="' + data.d[i] + '">' + data.d[i] + '</option>';
                    };
                    dropdown.append(option);

                    var param = { selectedBudget: data.d[0] };
                    if (param.selectedBudget == undefined) {
                        param.selectedBudget = '';
                    }
                                      
                    createActualvsBudgetedRevenue(param);
                },
                error: function (result) {
                    //show no data messages here
                }
            })
        }

    </script>

</div>
