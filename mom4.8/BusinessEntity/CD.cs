using System;
using System.Data;

namespace BusinessEntity
{
    public class CD
    {
        public int ID;
        public DateTime fDate;
        public long Ref;
        public string fDesc;
        public double Amount;
        public int Bank;
        public Int16 Type;
        public Int16 Status;
        public int TransID;
        public int Vendor;
        public string French;
        public string Memo;
        public string VoidR;
        public Int16 ACH;
        public int fDateYear;
        public bool IsRecon;
        public DateTime StartDate;
        public DateTime EndDate;
        public long NextC;
        private DataSet _ds;
        private string _ConnConfig;
        public bool IsExistCheckNo;
        private DataTable _dt;
        public int DiscGL;
        public string searchterm ;
        public string searchvalue;
        public string updateBy;
        public DateTime updateByValue;
        public bool isVH;
        public bool isDisc;
       
        public int EN { get; set; }

        public int UserID { get; set; }

        public DataTable Dt
        {
            get { return _dt; }
            set { _dt = value; }
        }
        public DataSet Ds
        {
            get { return _ds; }
            set { _ds = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public string MOMUSer { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }


    public class GetCDByIDParam
    {
        private string _ConnConfig;
        public int ID;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class UpdateCDVoidParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public string fDesc;
        public Int16 Status;
        public int ID;
    }

    public class GetAllCDParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DateTime StartDate;
        public DateTime EndDate;
        public string fDesc;
        public Int16 Status;
        public string searchterm;
        public int UserID { get; set; }
        public string searchvalue;
        public int EN { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

    }

    public class DeleteRecurrCheckParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int ID;
    }

    public class GetProcessRecurrCheckCountParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class DeleteCheckDetailsParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int ID;
    }

    public class GetCheckRecurrDetailsParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DateTime StartDate;
        public DateTime EndDate;
        public string searchterm;
        public int UserID { get; set; }
        public string searchvalue;
        public int EN { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class GetDataTypeCDParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class GetCheckDetailsByBankAndRefParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public long Ref;
        public long NextC;
        public int Bank;
    }

    public class UpdateCDVoidOpenParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public string fDesc;
        public Int16 Status;
        public double Amount;
        public int ID;
    }

    public class UpdateAPCDVoidLogParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public string French;
        public int ID;
        public string Memo;
        public string searchterm;
        public string searchvalue;
        public string MOMUSer { get; set; }
    }

    public class ProcessRecurCheckParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int ID;
        public string MOMUSer { get; set; }
    }

    public class GetRunningBalanceCountsParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class GetAutoSelectPaymentParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class AddCheckParam
    {
        private string _ConnConfig;
        private DataTable _dt;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DataTable Dt
        {
            get { return _dt; }
            set { _dt = value; }
        }
        public DateTime fDate;
        public long NextC;
        public string fDesc;
        public int Bank;
        public int Vendor;
        public string Memo;
        public int DiscGL;
        public Int16 Type;
        public string MOMUSer { get; set; }
    }

    public class ApplyCreditParam
    {
        private string _ConnConfig;
        private DataTable _dt;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DataTable Dt
        {
            get { return _dt; }
            set { _dt = value; }
        }
        public DateTime fDate;
        public long NextC;
        public string fDesc;
        public int Bank;
        public int Vendor;
        public string Memo;
        public int DiscGL;
        public Int16 Type;
        public string MOMUSer { get; set; }
    }

    public class AutoSelectPaymentParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public string updateBy;
        public DateTime updateByValue;
        public bool isVH;
        public bool isDisc;
    }

    public class AddCheckRecurrParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public double Amount;
        public int fDateYear;
        public int TransID;
        public DateTime fDate;
        public long NextC;
        public string fDesc;
        public int Bank;
        public int Vendor;
        public string Memo;
        public int DiscGL;
        public Int16 Type;
        public string MOMUSer { get; set; }

    }


    public class GetSelectedOpenAPPJIDParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class UpdateAPCDDateParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int ID;
        public DateTime fDate;
        public long Ref;
        public int Vendor;
        public string MOMUSer { get; set; }
    }

    public class GetAPCheckLogsParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        //public int ID;
        public long Ref;
    }

    public class GetFederalReportParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DateTime StartDate;
        public DateTime EndDate;
        public double Amount;
    }

    public class UpdateCDCheckNoParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int ID;
        public int Ref;
    }
    public class IsExistCheckNumParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public bool IsExistCheckNo;
        public long Ref;
        public int Bank;        
    }
    public class GetRecurCDByIDParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int ID;
    }
    public class UpdateCheckRecurrParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int ID;
        public double Amount;
        public int fDateYear;
        public int TransID;
        public DateTime fDate;
        public long NextC;
        public string fDesc;
        public int Bank;
        public int Vendor;
        public string Memo;
        public int DiscGL;
        public Int16 Type;
        public string MOMUSer { get; set; }
    }
    public class UpdateApplyCreditDateParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DateTime fDate;
        public DateTime EndDate;
        public int TransID;
    }

    public class IsExistCheckNumOnEditParam
    {
        private string _ConnConfig;
        public bool IsExistCheckNo;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public long Ref;
        public int Vendor;
        public int Bank;
        public int ID;
    }

}
