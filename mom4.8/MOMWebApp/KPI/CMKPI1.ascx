<%@ Control Language="C#" AutoEventWireup="true" Inherits="KPI_CMKPI1" Codebehind="CMKPI1.ascx.cs" %>
<div>
    <div id="equipmentKPI" style="display: block; height: 300px;">
    </div>
    <script>
        function createEquipmentTypeCount() {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "KPI/KPIWebService.asmx/GetEquipmentTypeCount",
                data: {},
                dataType: "json",
                success: function (data) {
                    createDonutChart($("#equipmentKPI"), data, "Material", "", "equipmentByType", 60, 80);
                },
                error: function (result) {
                    //show no data messages here
                }
            })
        }        
    </script>
</div>


