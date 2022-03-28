using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class ListGetEquipmentInspection
    {
        public List<GetEquipmentInspectionTable1> lstTable1 { get; set; }
        public List<GetEquipmentInspectionTable2> lstTable2 { get; set; }
    }

    [Serializable]
    public class GetEquipmentInspectionTable1
    {
        public string EquipmentName { get; set; }
        public string State { get; set; }
        public string ServiceType { get; set; }
        public string Category { get; set; }
        public string Manuf { get; set; }
        public double Price { get; set; }
        public DateTime LastService { get; set; }
        public DateTime Installed { get; set; }
        public string BuildingType { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Customer { get; set; }
        public string LocationID { get; set; }
        public string Location { get; set; }
        public string Address { get; set; }
        public string Shutdown { get; set; }
        public string Classification { get; set; }
        public string ShutdownReason { get; set; }

    }

    [Serializable]
    public class GetEquipmentInspectionTable2
    {
        public string Location { get; set; }
        public int OwnerID { get; set; }
        public string OwnerName { get; set; }
        public string equipment { get; set; }
        public string Unique { get; set; }
        public string AnnualInspDate { get; set; }
        public string InspectorName { get; set; }
        public string Balastrades { get; set; }
        public string BuildingELBI { get; set; }
        public string BuildingPower { get; set; }
        public string Capacity { get; set; }
        public string CarStation { get; set; }
        public string CarStationBulb { get; set; }
        public string CarStationMfg { get; set; }
        public string CarWeight { get; set; }
        public string CodeYear { get; set; }
        public string CoilPartNumber { get; set; }
        public string CounterweightRollerPartNumber { get; set; }
        public string DoorOpeningWidth { get; set; }
        public string DoorOperator { get; set; }
        public string DoorOperatorBelt { get; set; }
        public string DoorRollerPartNumber { get; set; }
        public string DoorType { get; set; }
        public string DueDate { get; set; }
        public string EmergencyBatteryCharger { get; set; }
        public string Emergencylightbattery { get; set; }
        public string EmergencyLightBulb { get; set; }
        public string EscalatorCombTeeth { get; set; }
        public string EscalatorHandrail { get; set; }
        public string EscalatorHandrailColor { get; set; }
        public string EscalatorHandrailLength { get; set; }
        public string EscalatorHandrailReplacementDate { get; set; }
        public string EscalatorRollerPartNumber { get; set; }
        public string EscalatorSkirtSwitch { get; set; }
        public string EscalatorSpeed { get; set; }
        public string EscalatorStepWidth { get; set; }
        public string FireServiceKey { get; set; }
        public string FireServicePhaseII { get; set; }
        public string Floors { get; set; }
        public string FPM { get; set; }
        public string FiveYearInspDate { get; set; }
        public string GateSwitchPartNumber { get; set; }
        public string GeneratorBrushPartNumber { get; set; }
        public string GeneratorMfg { get; set; }
        public string GibPartNumber { get; set; }
        public string GovernorMfg { get; set; }
        public string GovernorRopeLength { get; set; }
        public string GovernorRopeSize { get; set; }
        public string GuideRollerPartNumber { get; set; }
        public string HallLanternLensCap { get; set; }
        public string HallLanternMfg { get; set; }
        public string HallStationbulb { get; set; }
        public string HallStationManufacturer { get; set; }
        public string HoistwayAccessSwitch { get; set; }
        public string InputBoard { get; set; }
        public string InterlockPartNumber { get; set; }
        public string MfgJobNumber { get; set; }
        public string ModelType { get; set; }
        public string MotorRPM { get; set; }
        public string MotorType { get; set; }
        public string OilLineSize { get; set; }
        public string Openings { get; set; }
        public string OutputBoard { get; set; }
        public string PackingNumber { get; set; }
        public string PistonDiameter { get; set; }
        public string PositionIndicatorBulb { get; set; }
        public string PumpMotorMfg { get; set; }
        public string PumpUnitMfg { get; set; }
        public string PumpMotorBelt { get; set; }
        public string purchaseDate { get; set; }
        public string AlteredDate { get; set; }
        public string RopeLength { get; set; }
        public string RopeSize { get; set; }
        public string RefNo { get; set; }
        public string SafetyEdges { get; set; }
        public string SerialNo { get; set; }
        public string ServiceInterval { get; set; }
        public string ServiceIntervalUnit { get; set; }
        public string SpiratorPartNumber { get; set; }
        public string StartType { get; set; }
        public string StarterContactMfgandPartNo { get; set; }
        public string TimeClock { get; set; }
        public string TimeClockModel { get; set; }
        public string TXE { get; set; }
        public string ValveMfg { get; set; }
        public string WarrantyExpirationDate { get; set; }
        public string WherePurchased { get; set; }
        public string AnnualInspectionViolations { get; set; }
        public string AnnualInspectorCustomerPreference { get; set; }

    }
}
