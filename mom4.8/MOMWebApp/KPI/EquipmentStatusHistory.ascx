<%@ Control Language="C#" AutoEventWireup="true" Inherits="KPI_EquipmentStatusHistory" Codebehind="EquipmentStatusHistory.ascx.cs" %>
<div>
    <div id="equipmentStatusHistory" style="display: block; height: 300px;">
    </div>
    <script>

        function createEquipmentStatusHistory() {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "KPI/KPIWebService.asmx/GetLocationsStatus",
                data: {},
                dataType: "json",
                success: function (data) {
                    createBarChart($("#equipmentStatusHistory"), data, "Active VS Under Contract Locations")
                },
                error: function (result) {
                    //show no data messages here
                }
            })
        }
    </script>
</div>
