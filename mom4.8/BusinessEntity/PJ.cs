using System;
using System.Data;

namespace BusinessEntity
{
    public class PJ
    {
       public int ID;
        public DateTime fDate;
        public DateTime PostDate;
        public string Ref;
        public string fDesc;
        public double Amount;
        public double Balance;
        public int Vendor;
        public Int16 Status;
        
        public int Batch;
        public Int16 Terms;
        public int PO;
        public int TRID;
        public Int16 Spec;
        public int IfPaid;
        public DateTime IDate;
        public double UseTax;
        public double Disc;
        public string Custom1;
        public string Custom2;
        public int ReqBy;
        public string VoidR;
        public string UtaxName;
        public int GL;
        private DataSet _ds;
        private string _ConnConfig;
        public DateTime StartDate;
        public DateTime EndDate;
        public Int16 SearchValue;
        public DateTime SearchDate;
        public DataTable _dt;
        public int ReceivePo;
        public DateTime Due;
        public int ProjectNumber;
        public string vendorName;
        public bool IsRecurring;
        public int Frequency;
        private string _searchwithmultiplestatus;
        public string SearchwithmultipleStatus
        {
            get
            {
                return _searchwithmultiplestatus;
            }
            set
            {
                _searchwithmultiplestatus = value;
            }
        }
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
        public string Custom;

        public string STaxName;
        public string UTaxName;
        public int STaxGL;
        public int UTaxGL;
        public double STax;
        public double STaxRate;
        public double UTax;
        public double UTaxRate;
        public double GST;
        public int GSTGL;
        public double GSTRate;
        public bool IsPOClose;
        public string VendorType;
    }


    public class GetPJDetailByBatchParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int Batch;
    }

    public class AddPJParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DateTime fDate;
        public string Ref;
        public string fDesc;
        public double Amount;
        public int Vendor;
        public Int16 Status;
        public int Batch;
        public Int16 Terms;
        public int PO;
        public int ReceivePo;
        public int TRID;
        public Int16 Spec;
        public DateTime IDate;
        public double Disc;
        public string Custom1;
        public string Custom2;
        public double UseTax;
        public int ReqBy;
        public string VoidR;
    }

    public class UpdatePJOnVoidCheckParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int ID;
        public string fDesc;
        public Int16 Status;
    }

    public class AddBillsParam
    {
        private string _ConnConfig;
        public DataTable Dt;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int Vendor;
        public DateTime fDate;
        public DateTime PostDate;
        public string Ref;
        public DateTime Due;
        public string fDesc;
        public Int16 Terms;
        public Int16 Spec;
        public string MOMUSer { get; set; }
        //public DataTable Dt
        //{
        //    get { return _dt; }
        //    set { _dt = value; }
        //}
        public Int16 Status;
        public bool IsRecurring;
        public int Frequency;
        public int PO;
        public int ReceivePo;
        public double Disc;
        public string Custom1;
        public string Custom2;
        public int IfPaid;
        public double STax;
        public string STaxName;
        public string UTaxName;
        public int STaxGL;
        public int UTaxGL;
        public double STaxRate;
        public double UTax;
        public double UTaxRate;
        public double GST;
        public int GSTGL;
        public double GSTRate;
        public bool IsPOClose;
    }

    public class GetAllPJDetailsParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        //public Int32 ID;
        public int UserID { get; set; }
        public DateTime StartDate;
        public DateTime EndDate;
        //public string StartDate;
        //public string EndDate;
        public int PO;
        public int ProjectNumber;
        public double Amount;
        public int Vendor;
        public string vendorName;
        public string Custom1;
        public string Custom2;
        public double Balance;
        public Int16 Terms;
        public DateTime SearchDate;
        public Int16 SearchValue;
        //public int TRID;
        //public DateTime Due;
        //public DateTime PostDate;
        //public DateTime IDate;
        //public int Batch;
        public int EN { get; set; }
        public string Ref;

        public Int16 Status;
        public string VendorType;

        private string _searchwithmultiplestatus;
        public string SearchwithmultipleStatus
        {
            get
            {
                return _searchwithmultiplestatus;
            }
            set
            {
                _searchwithmultiplestatus = value;
            }
        }

    }

    public class GetAllPJRecurrDetailsParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public Int32 ID;
        public int UserID { get; set; }
        public DateTime StartDate;
        public DateTime EndDate;
        public int PO;
        public int ProjectNumber;
        public double Amount;
        public int Vendor;
        public string vendorName;
        public string Custom1;
        public string Custom2;
        public double Balance;
        public Int16 Terms;
        public DateTime SearchDate;
        public Int16 SearchValue;
        public int TRID;
        public DateTime Due;
        public DateTime PostDate;
        public DateTime IDate;
        public int Batch;
        public int EN { get; set; }
        public string Ref;

        public Int16 Status;
    }

    public class ProcessRecurBillParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public Int32 ID;

    }
    public class DeleteAPBillRecurrParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public Int32 ID;

    }
    public class GetProcessRecurrCountParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public Int32 ID;

    }

    public class DeleteAPBillParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public Int32 ID;

    }
    public class UpdateAPDatesParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public Int32 ID;
        public int TRID;
        public DateTime Due;
        public DateTime PostDate;
        public DateTime IDate;
        public int Batch;

    }
    public class GetPJAcctDetailByIDParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public Int32 ID;

    }

    public class GetPJDetailByIDParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public Int32 ID;

    }

    public class GetBillTransDetailsParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public Int32 ID;
        public int Batch;
    }
    public class GetPJRecurrDetailByIDParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public Int32 ID;
        public int Batch;
    }

    public class GetBillRecurrTransactionsParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public Int32 ID;
        public int Batch;
    }

    public class GetBillHistoryPaymentParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }

        public Int32 ID;
        public int Batch;
    }

    public class GetAllPJDetailsForReportsParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DateTime StartDate;
        public DateTime EndDate;
        
        public Int16 Terms;
        public DateTime SearchDate;
        public Int16 SearchValue;
    }

    public class GetBillsDetailsByDueParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int UserID { get; set; }
        public DateTime SearchDate;
        public int EN { get; set; }
        public Int16 SearchValue;
        public int Vendor;
        public int Frequency;
    }
    public class GetAPAgingByDateParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int UserID { get; set; }
        public DateTime SearchDate;
        public int EN { get; set; }
        public DateTime fDate;
        public int Frequency;
    }
    public class GetUseTaxForReportsParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DateTime StartDate;
        public DateTime EndDate;

    }
    public class GetAPGLRegParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DateTime StartDate;
        public DateTime EndDate;

    }
    public class UpdateRecurrBillsParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int Vendor;
        public DateTime fDate;
        public DateTime PostDate;
        public DateTime Due;
        public string Ref;
        public string fDesc;
        public Int16 Terms;
        public Int16 Spec;
        public string MOMUSer { get; set; }
        public DataTable Dt;

        //public DataTable Dt
        //{
        //    get { return _dt; }
        //    set { _dt = value; }
        //}
        public Int16 Status;
        public bool IsRecurring;
        public int Frequency;
        public int ID;
        public int PO;
        public int ReceivePo;
        public double Disc;
        public string Custom1;
        public string Custom2;
        public int IfPaid;
        public string STaxName;
        public string UTaxName;
        public int STaxGL;
        public int UTaxGL;
        public double STax;
        public double STaxRate;
        public double UTax;
        public double UTaxRate;
        public double GST;
        public int GSTGL;
        public double GSTRate;
    }

    public class UpdateBillsParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DataTable Dt;
        //public DataTable Dt
        //{
        //    get { return _dt; }
        //    set { _dt = value; }
        //}

        public int ID;
        public int Vendor;
        public DateTime fDate;
        public DateTime PostDate;
        public DateTime Due;
        public string Ref;
        public string fDesc;
        public Int16 Terms;
        public int PO;
        public int ReceivePo;

        public Int16 Status;
        public string Custom1;
        public string Custom2;
        public double Disc;
        public int Batch;
        public int TRID;
        public string MOMUSer { get; set; }
        public int IfPaid;
        public string STaxName;
        public string UTaxName;
        public int STaxGL;
        public int UTaxGL;
        public double STax;
        public double STaxRate;
        public double UTax;
        public double UTaxRate;
        public double GST;
        public int GSTGL;
        public double GSTRate;
        public bool IsPOClose;

    }
    public class UpdateBillsJobDetailsParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DataTable Dt;

        //public DataTable Dt
        //{
        //    get { return _dt; }
        //    set { _dt = value; }
        //}
        public DateTime fDate;
        public string Ref;
        public int Batch;
    }

    public class GetBillingItemsParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DataTable Dt;
        //public DataTable Dt
        //{
        //    get { return _dt; }
        //    set { _dt = value; }
        //}
        public int EN { get; set; }
        public int UserID { get; set; }
    }

    public class GetBillsLogsParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int ID;
    }

    public class GetBillsDetails360ByDueParam
    {
        private string _ConnConfig;
        private DataSet _ds;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public Int16 SearchValue { get; set;}
        public DateTime SearchDate { get; set; }
        public int Vendor { get; set; }
        public DataSet Ds
        {
            get { return _ds; }
            set { _ds = value; }
        }
    }

   public class GetAPAging360ByDateParam
    {
        private string _ConnConfig;
        private DataSet _ds;
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
        public DateTime fDate { get; set; }
        public int EN { get; set; }
        public int UserID { get; set; }
        public DateTime SearchDate { get; set; }
    }

}
