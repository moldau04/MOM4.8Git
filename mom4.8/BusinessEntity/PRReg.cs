using System;
using System.Data;

namespace BusinessEntity
{
    public class PRReg
    {
        public int RegisterId;
        public int ID;
        public DateTime fDate;
        public DateTime CDate;
        public int Ref;
        public int fDesc;
        public int EmpID;
        public int Bank;
        public int TransID;
	    public double Reg;
	    public double YReg;
        public double HReg;
        public double HYReg;
        public double OT;
        public double YOT;
        public double HOT;
        public double HYOT;
        public double DT;
        public double YDT;
        public double HDT;
        public double HYDT;
        public double TT;
        public double YTT;
        public double HTT;
        public double HYTT;
        public double Hol;
        public double YHol;
        public double HHol;
        public double HYHol;
        public double Vac;
        public double YVac;
        public double HVac;
        public double HYVac;
        public double Zone;
        public double YZone;
        public double Reimb;
        public double YReimb;
        public double Mile;
        public double YMile;
        public double HMile;
        public double HYMile;
        public double Bonus;
        public double YBonus;
        public double WFIT;
        public double WFica;
        public double WMedi;
        public double WFuta;
        public double WSit;
        public double WVac;
        public double WWComp;
        public double WUnion;
        public double FIT;
        public double YFIT;
        public double FICA;
        public double YFICA;
        public double MEDI;
        public double YMEDI;
        public double FUTA;
        public double YFUTA;
        public double SIT;
        public double YSIT;
        public double Local;
        public double YLocal;
        public double TOTher;
        public double NT;
        public double YTOTher;
        public double TInc;
        public double YNT;
        public double HNT;
        public double TDed;
        public double HYNT;
        public double Net;
        public double State;
        public double VThis;
        public double REIMJE;
        public double WELF;
        public double SDI;
	    public double K401;
	    public double GARN;
        public double WeekNo;
        public double Remarks;
        public double ELast;
        public double EThis;
        public double CompMedi;
        public double WMediOverTH;
        public double Sick;
        public double YSick;
        public double WSick;
        public double HSick;
        public double HYSick;
        public double HSickAccrued;
        public double HYSickAccrued;
        public double HVacAccrued;
        public double HYVacAccrued;
        private DataSet _ds;
        private string _ConnConfig;
        public DateTime StartDate;
        public DateTime EndDate;
        public Int16 SearchValue;
        public DateTime SearchDate;
        public DataTable _dt;
        public long stRef;
        public long edRef;

        public int EN { get; set; }
        public int UserID { get; set; }
        public DataTable Dt
        {
            get { return _dt; }
            set { _dt = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DataSet Ds
        {
            get { return _ds; }
            set { _ds = value; }
        }
        public string MOMUSer { get; set; }
        public string Memo;
        public string Description;
        public string ProcessMethod;
        public string Supervisor;
        public int PrcessDed;
        public long Checkno;

        public int SupervisorId;
        public int FrequencyId;
        public int DepartmentId;
        public double NetPay;
        public double TotalDeduction;
        public double GrossPay;

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
        public string SortType { get; set; }

    }

    
}
