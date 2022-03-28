<%@ Control Language="C#" AutoEventWireup="true" Inherits="KPI_AvgEstimateConversionRate" Codebehind="AvgEstimateConversionRate.ascx.cs" %>


<div id="avgEstimate" style="display: inline-block;">
    </div>
    <script>

        function createAvgEstimateConversionRate() {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "KPI/KPIWebService.asmx/AvgEstimate",
                data: {},
                dataType: "json",
                success: function (data) {
                    $("#avgEstimate").text(data.d[0].toFixed(0)); 
                    if (data.d[2] == 'Increment') {
                        $("#imgUpAvgEstimate").show();
                        $("#imgDownAvgEstimate").hide();
                        $('#AvgEstimateUpByDownBy').text(data.d[3]);
                    }
                    else {
                        $("#imgUpAvgEstimate").hide();
                        $("#imgDownAvgEstimate").show();
                        $('#AvgEstimateUpByDownBy').text(data.d[3]);
                    }
                },
                error: function (result) {
                    //show no data messages here
                }
            });
        }


    </script>

