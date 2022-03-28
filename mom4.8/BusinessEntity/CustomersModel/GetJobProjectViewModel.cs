using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetJobProjectViewModel
    {
        public int ID { get; set; }
        public string LID { get; set; }
        public string fDesc { get; set; }
        public string Type { get; set; }
        public int Loc { get; set; }
        public string LocationType { get; set; }
        public string BuildingType { get; set; }
        public string tag { get; set; }
        public string Address { get; set; }
        public int Owner { get; set; }
        public int Elev { get; set; }
        public string Status { get; set; }
        public string PO { get; set; }
        public double Rev { get; set; }
        public double Mat { get; set; }
        public double OtherExp { get; set; }
        public double Labor { get; set; }
        public double Cost { get; set; }
        public double Profit { get; set; }
        public double Ratio { get; set; }
        public double Reg { get; set; }
        public double OT { get; set; }
        public double DT { get; set; }
        public double TT { get; set; }
        public double Hour { get; set; }
        public double BRev { get; set; }
        public double BMat { get; set; }
        public double BLabor { get; set; }
        public double BCost { get; set; }
        public double BProfit { get; set; }
        public double BRatio { get; set; }
        public double BHour { get; set; }
        public int Template { get; set; }
        public DateTime fDate { get; set; }
        public DateTime CloseDate { get; set; }
        public double Comm { get; set; }
        public int WageC { get; set; }
        public double NT { get; set; }
        public Int16 Post { get; set; }
        public int EN { get; set; }
        public string Company { get; set; }
        public Int16 Certified { get; set; }
        public Int16 Apprentice { get; set; }
        public Int16 UseCat { get; set; }
        public Int16 UseDed { get; set; }
        public double BillRate { get; set; }
        public double Markup { get; set; }
        public Int16 PType { get; set; }
        public Int16 Charge { get; set; }
        public int Amount { get; set; }
        public int GL { get; set; }
        public int GLRev { get; set; }
        public double GandA { get; set; } 
        public double OHLabor { get; set; }
        public double LastOH { get; set; }
        public double etc { get; set; }
        public double ETCModifier { get; set; }
        public string FP { get; set; }
        public string fGroup { get; set; }
        public string CType { get; set; }
        public int Elevs { get; set; }
        public double RateTravel { get; set; }
        public double RateOT { get; set; }
        public double RateNT { get; set; }
        public double RateDT { get; set; }
        public double RateMileage { get; set; }
        public Int16 NType { get; set; }
        public string Custom1 { get; set; }
        public string Custom2 { get; set; }
        public string Custom3 { get; set; }
        public string Custom4 { get; set; }
        public string Custom5 { get; set; }
        public string Custom6 { get; set; }
        public DateTime StartDate { get; set; }
        public string Customer { get; set; }
        public string Remarks { get; set; }
        public string TemplateDesc { get; set; }
        public string Salesperson { get; set; }
        public string Route { get; set; }
        public double NHour { get; set; }
        public double NLabor { get; set; }
        public double NMat { get; set; }
        public double NOtherExp { get; set; }
        public double NTicketOtherExp { get; set; }
        public double NCost { get; set; }
        public double NRev { get; set; }
        public double NotBilledYet { get; set; }
        public double NComm { get; set; }
        public double ReceivePO { get; set; }
        public double NProfit { get; set; }
        public double NRatio { get; set; }
        public int Bill { get; set; }
        public int BillPercent { get; set; }
        public string Url { get; set; }
        public double ContractPrice { get; set; }
        public double TotalBudgetedExpense { get; set; }
        public int ProjectManagerUserID { get; set; }
        public int AssignedProjectUserID  { get; set;}
        public string ProjectManagerUserName { get; set; }
        public double Nomat { get; set; }
    }
}
