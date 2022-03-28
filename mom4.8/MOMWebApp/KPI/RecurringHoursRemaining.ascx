<%@ Control Language="C#" AutoEventWireup="true" Inherits="KPI_RecurringHoursRemaining" Codebehind="RecurringHoursRemaining.ascx.cs" %>

<div>
    <div id="recurringHoursRemaining" style="display: block; height: 400px;">
    </div>

    <script>

        function createRecurringHoursRemaining() {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "KPI/KPIWebService.asmx/GetEstimatedAndTotalHoursCompleted",
                data: {},
                dataType: "json",
                success: function (data) {
                    createBarChart3Bars($("#recurringHoursRemaining"), data, "Recurring Hours Remaining");
                },
                error: function (result) {
                    //show no data messages here
                }
            })
        }

    </script>

</div>
