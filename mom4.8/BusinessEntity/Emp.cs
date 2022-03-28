using System;
using System.Data;

namespace BusinessEntity
{
    public class Emp
    {
        private string _ConnConfig;
        private string _DBName;
        public DataSet _ds;
        public DataTable _dt;
        public DataTable _dtWageCategory;
        public DataTable _dtWageDeduction;
        public DataTable _dtOtherIncome;
        
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public string DBName
        {
            get { return _DBName; }
            set { _DBName = value; }
        }
        public DataSet Ds
        {
            get { return _ds; }
            set { _ds = value; }
        }
        public DataTable Dt
        {
            get { return _dt; }
            set { _dt = value; }
        }
        public DataTable dtWageCategory
        {
            get { return _dtWageCategory; }
            set { _dtWageCategory = value; }
        }
        public DataTable dtWageDeduction
        {
            get { return _dtWageDeduction; }
            set { _dtWageDeduction = value; }
        }
        public DataTable dtOtherIncome
        {
            get { return _dtOtherIncome; }
            set { _dtOtherIncome = value; }
        }
        public int ID { get; set; }



        public string fFirst { get; set; }
        public string Last { get; set; }
        public string Middle { get; set; }
        public string Name { get; set; }
        public int Rol { get; set; }
        public string SSN { get; set; }
        public string Title { get; set; }
        public int Sales { get; set; }
        public int Field { get; set; }
        public int Status { get; set; }
        public string Pager { get; set; }
        public int InUse { get; set; }
        public int PayPeriod { get; set; }
        public DateTime DHired { get; set; }
        public DateTime DFired { get; set; }
        public DateTime DBirth { get; set; }
        public DateTime DReview { get; set; }
        public DateTime DLast { get; set; }
        public int FStatus { get; set; }
        public int FAllow { get; set; }
        public double FAdd { get; set; }
        public int SStatus { get; set; }
        public int SAllow { get; set; }
        public double SAdd { get; set; }
        public string CallSign { get; set; }
        public double VRate { get; set; }
        public int VBase { get; set; }
        public double VLast { get; set; }
        public double VThis { get; set; }
        public double Sick { get; set; }
        public int PMethod { get; set; }
        public int PFixed { get; set; }
        public double PHour { get; set; }
        public int LName { get; set; }
        public int LStatus { get; set; }
        public int LAllow { get; set; }
        public int PRTaxE { get; set; }
        public string State { get; set; }
        public double Salary { get; set; }
        public int SalaryF { get; set; }
        public int SalaryGL { get; set; }
        public int fWork { get; set; }
        public int NPaid { get; set; }
        public double Balance { get; set; }
        public double PBRate { get; set; }
        public double FITYTD { get; set; }
        public double FICAYTD { get; set; }
        public double MEDIYTD { get; set; }
        public double FUTAYTD { get; set; }
        public double SITYTD { get; set; }
        public double LocalYTD { get; set; }
        public double BonusYTD { get; set; }
        public double HolH { get; set; }
        public double HolYTD { get; set; }
        public double VacH { get; set; }
        public double VacYTD { get; set; }
        public double ZoneH { get; set; }
        public double ZoneYTD { get; set; }
        public double ReimbYTD { get; set; }
        public double MileH { get; set; }
        public double MileYTD { get; set; }
        public string Race { get; set; }
        public string Sex { get; set; }
        public string Ref { get; set; }
        public int ACH { get; set; }
        public int ACHType { get; set; }
        public string ACHRoute { get; set; }
        public string ACHBank { get; set; }
        public DateTime Anniversary { get; set; }
        public int Level { get; set; }
        public int WageCat { get; set; }
        public DateTime DSenior { get; set; }
        public int PRWBR { get; set; }
        public string PDASerialNumber_1 { get; set; }
        public int StatusChange { get; set; }
        public DateTime SCDate { get; set; }
        public string SCReason { get; set; }
        public int DemoChange { get; set; }
        public string Language { get; set; }
        public int TicketD { get; set; }
        public string Custom1 { get; set; }
        public string Custom2 { get; set; }
        public string Custom3 { get; set; }
        public string Custom4 { get; set; }
        public string Custom5 { get; set; }
        public int DDType { get; set; }
        public double DDRate { get; set; }
        public int ACHType2 { get; set; }
        public string ACHRoute2 { get; set; }
        public string ACHBank2 { get; set; }
        public double BillRate { get; set; }
        public double BMSales { get; set; }
        public double BMInvAve { get; set; }
        public double BMClosing { get; set; }
        public double BMBillEff { get; set; }
        public double BMProdEff { get; set; }
        public int BMAveTask { get; set; }
        public int BMCustom1 { get; set; }
        public int BMCustom2 { get; set; }
        public int BMCustom3 { get; set; }
        public int BMCustom4 { get; set; }
        public int BMCustom5 { get; set; }
        public string TaxCodeNR { get; set; }
        public string TaxCodeR { get; set; }
        public string DeviceID { get; set; }
        public double MileageRate { get; set; }
        public string Import1 { get; set; }
        public string MSDeviceId { get; set; }
        public string PayPortalPassword { get; set; }

        public double SickRate { get; set; }
        public double SickAccrued { get; set; }
        public double SickUsed { get; set; }
        public double SickYTD { get; set; }
        public double VacAccrued { get; set; }
        public int SCounty { get; set; }
        public string SearchValue { get; set; }
        public string TechnicianBio { get; set; }
        public string PDASerialNumber { get; set; }
        public string Supervisor { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public string Fax { get; set; }        
        public string Zip { get; set; }
        public string Tel { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Cell { get; set; }
        public string Remarks { get; set; }
        public int Type { get; set; }
        public string Contact { get; set; }
        public string Website { get; set; }
        public string EmployeeID { get; set; }
        public string Geocode { get; set; }
        public string FillingState { get; set; }
        public string MOMUSer { get; set; }

    }
}
