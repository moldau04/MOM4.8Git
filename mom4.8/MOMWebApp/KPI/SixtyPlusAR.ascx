<%@ Control Language="C#" AutoEventWireup="true" Inherits="KPI_SixtyPlusAR" Codebehind="SixtyPlusAR.ascx.cs" %>
<div>
    <div id="sixtyPlusAR">
        <div class="row">
            <div class="col s2">
                <img id="imgUp" src="../images/upArrow.png" height="30px" width="30px">
                <img id="imgDown" src="../images/downArrow.png" height="30px" width="30px">
            </div>
            <div class="col s4" id="SixtyPlusARcounts"></div>
        </div>
        <div class="row">
            <div class="col s4">+60 AR</div>
        </div>

        <script>
            function getPercentageChange(oldNumber, newNumber) {
                var decreaseValue = oldNumber - newNumber;

                return (decreaseValue / oldNumber) * 100;
            }

            function createGetSixtyPlusAR() {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "KPI/KPIWebService.asmx/GetSixtyPlusAR",
                    data: {},
                    dataType: "json",
                    success: function (data) {
                        $("#SixtyPlusARcounts").text(data.d[1].toFixed(2))
                        var Percent = getPercentageChange(data.d[1], data.d[0])
                        if (data.d[0] > data.d[1]) {
                            $("#imgUp").hide()
                            $("#imgDown").show()
                        }
                        else {
                            $("#imgUp").show()
                            $("#imgDown").hide()
                        }
                    },
                    error: function (result) {
                       //show no data messages here
                    }
                });
            };
        </script>
    </div>
</div>
