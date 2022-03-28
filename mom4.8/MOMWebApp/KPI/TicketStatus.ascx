<%@ Control Language="C#" AutoEventWireup="true" Inherits="KPI_TicketStatus" Codebehind="TicketStatus.ascx.cs" %>
<div>
    <div id="ticketStatus">
        <div id="counts"></div>
        <script>
            function getTicketStatus() {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "KPI/KPIWebService.asmx/GetTicketStatus",
                    data: {},
                    dataType: "json",
                    success: function (data) {
                        $("#counts").text(data.d[1] + "/" + data.d[0])
                    },
                    error: function (result) {
                        //show no data messages here
                    }
                });
            }
        </script>
    </div>
</div>
