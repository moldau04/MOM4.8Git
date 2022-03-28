<%@ Control Language="C#" AutoEventWireup="true" Inherits="KPI_EquipmentByBuilding" Codebehind="EquipmentByBuilding.ascx.cs" %>

<div>
    <div id="equipmentByBuilding" style="display: block; height: 300px;">
        <script>
            function createEquipmentBuildingCount() {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "KPI/KPIWebService.asmx/GetEquipmentBuildingCount",
                    data: {},
                    dataType: "json",
                    success: function (data) {
                        createDonutChart($("#equipmentByBuilding"), data, "Material", "Equipment By Building", "equipmentByBuilding", 60, 60);
                    },
                    error: function (result) {
                        //show no data messages here
                    }
                })
            } 
        </script>
    </div>
</div>
