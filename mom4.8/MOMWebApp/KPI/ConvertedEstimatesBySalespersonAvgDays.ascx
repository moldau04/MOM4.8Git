<%@ Control Language="C#" AutoEventWireup="true" Inherits="KPI_ConvertedEstimatesBySalespersonAverageDays" Codebehind="ConvertedEstimatesBySalespersonAvgDays.ascx.cs" %>

<div class="container">
  
    <div class="row">
        <div id="convertedEstimatesBySalespersonAvgDays" style="display: block;height:300px">
        </div>
    </div>
    <script>
          
        function convertedEstimatesBySalespersonAvgDays() {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "KPI/KPIWebService.asmx/ConvertedEstimatesBySalespersonAverageDays",               
                dataType: "json",
                success: function (data) {
                    createBarChart1Bars($("#convertedEstimatesBySalespersonAvgDays"), data, "Actual vs Budgeted Revenue");
                },
                error: function (result) {
                    //alert(result.responseText);
                }
            })
        }
        
    </script>

</div>
