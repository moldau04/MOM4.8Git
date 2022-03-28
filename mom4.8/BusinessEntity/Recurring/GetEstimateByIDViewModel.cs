using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Recurring
{
    [Serializable]
    public class ListGetEstimateByID
    {
        public List<GetEstimateByIDTable1> lstTable1 { get; set; }
        public List<GetEstimateByIDTable2> lstTable2 { get; set; }
        public List<GetEstimateByIDTable3> lstTable3 { get; set; }
        public List<GetEstimateByIDTable4> lstTable4 { get; set; }
        public List<GetEstimateByIDTable5> lstTable5 { get; set; }
        public List<GetEstimateByIDTable6> lstTable6 { get; set; }
    }

    [Serializable]
    public class GetEstimateByIDTable1
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string fDesc { get; set; }
        public string CompanyName { get; set; }
        public string Remarks { get; set; }
        public int rolid { get; set; }
        public int locid { get; set; }
        public string LocationName { get; set; }
        public string Contact { get; set; }
        public string Category { get; set; }
        public int Opportunity { get; set; }
        public DateTime fDate { get; set; }
        public double cadexchange { get; set; }
        public Int16 status { get; set; }
        public int job { get; set; }
        public int Template { get; set; }
        public string EstimateBillAddress { get; set; }
        public DateTime BDate { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public int EstimateUserId { get; set; }
        public string EstimateAddress { get; set; }
        public string EstimateEmail { get; set; }
        public string EstimateCell { get; set; }
        public string JobType { get; set; }
        public double Cont { get; set; }
        public double BidPrice { get; set; }
        public double FinalBid { get; set; }
        public double OH { get; set; }
        public double OHPer { get; set; }
        public double MarkupPer { get; set; }
        public double MarkupVal { get; set; }
        public double CommissionPer { get; set; }
        public double CommissionVal { get; set; }
        public string STax { get; set; }
        public double STaxRate { get; set; }
        public string STaxName { get; set; }
        public double ContPer { get; set; }
        public Int16 PType { get; set; }
        public double Amount { get; set; }
        public double BillRate { get; set; }
        public double OT { get; set; }
        public double RateTravel { get; set; }
        public double DT { get; set; }
        public double RateMileage { get; set; }
        public double RateNT { get; set; }
        public string ffor { get; set; }
        public DateTime EstimateDate { get; set; }
        public bool Discounted { get; set; }
        public string DiscountedNotes { get; set; }
        public int ProspectID { get; set; }
        public string GroupName { get; set; }
        public int GroupId { get; set; }
        public string EstimateType { get; set; }
        public bool IsSglBilAmt { get; set; }
    }

    [Serializable]
    public class GetEstimateByIDTable2
    {
        public int ID { get; set; }
        public int Line { get; set; }
        public int TemplateID { get; set; }
        public int LabourID { get; set; }
        public double Amount { get; set; }
    }

    [Serializable]
    public class GetEstimateByIDTable3
    {
        public int id { get; set; }
        public string fdesc { get; set; }
        public string status { get; set; }
        public Int16 jStatus { get; set; }
        public string TemplateRev { get; set; }
        public int Count { get; set; }
        public Int16 Type { get; set; }
    }

    [Serializable]
    public class GetEstimateByIDTable4
    {
        public int ID { get; set; }
        public string JCode { get; set; }
        public string CodeDesc { get; set; }
        public int Line { get; set; }
        public string fDesc { get; set; }
        public Int16 JType { get; set; }
        public string MilesName { get; set; }
        public DateTime RequiredBy { get; set; }
        public DateTime ActAcquistDate { get; set; }
        public string Comments { get; set; }
        public int Type { get; set; }
        public string Department { get; set; }
        public double Amount { get; set; }
        public int Line1 { get; set; }
        public int EstimateItemID { get; set; }
        public string AmountPer { get; set; }
        public int OrderNo { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
    }

    [Serializable]
    public class GetEstimateByIDTable5
    {
        public int JobT { get; set; }
        public int Job { get; set; }
        public int JobTItemID { get; set; }
        public string fDesc { get; set; }
        public string Code { get; set; }
        public string CodeDesc { get; set; }
        public int Line { get; set; }
        public Int16 BType { get; set; }
        public double QtyReq { get; set; }
        public string UM { get; set; }
        public double BudgetUnit { get; set; }
        public double BudgetExt { get; set; }
        public int MatItem { get; set; }
        public double MatMod { get; set; }
        public double MatPrice { get; set; }
        public double MatMarkup { get; set; }
        public Int16 STax { get; set; }
        public string Currency { get; set; }
        public int LabItem { get; set; }
        public string MatName { get; set; }
        public double LabMod { get; set; }
        public double LabExt { get; set; }
        public double LabRate { get; set; }
        public double LabHours { get; set; }
        public DateTime SDate { get; set; }
        public string VendorId { get; set; }
        public string Vendor { get; set; }
        public double TotalExt { get; set; }
        public double LabPrice { get; set; }
        public double LabMarkup { get; set; }
        public Int16 LSTax { get; set; }
        public int EstimateItemID { get; set; }
        public int OrderNo { get; set; }
    }

    [Serializable]
    public class GetEstimateByIDTable6
    {
        public string AssignTo { get; set; }
        public string Address { get; set; }
        public double QuotedPrice { get; set; }
    }
}
