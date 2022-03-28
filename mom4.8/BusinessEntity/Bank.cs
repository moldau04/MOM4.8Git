using System;
using System.Data;

namespace BusinessEntity
{
    public class Bank
    {
        public int ID;
        public string fDesc;
        public int Rol;
        public string NBranch;
        public string NAcct;
        public string NRoute;
        public long NextC;
        public int NextD;
        public int NextE;
        public double Rate;
        public double CLimit;
        public Int16 Warn;
        public double Recon;
        public double Balance;
        public int Status;
        public int InUse;
        public int GeoLock = 0;
        private DataSet _dsBank;
        private string _ConnConfig;
        public int Chart;
        public DateTime LastReconDate;
        public double ServiceCharge;
        public double InterestCharge;
        public int ServiceAcct;
        public int InterestAcct;
        public DateTime ServiceDate;
        public DateTime InterestDate;
        public DataTable _dtBank;
        public DateTime fDate;
        public int CheckNoFrom;
        public int CheckNoTo;

        public string ACHFileHeaderStringA;
        public string ACHFileHeaderStringB;
        public string ACHFileHeaderStringC;
        public string ACHCompanyHeaderString1;
        public string ACHCompanyHeaderString2;
        public string ACHBatchControlString1;
        public string ACHBatchControlString2;
        public string ACHBatchControlString3;
        public string ACHFileControlString1;
        public string APACHCompanyID;
        public string APImmediateOrigin;
        public string NextACH;

        public string TraceNo1;
        public string TraceNo2;
        public string RecordTypeCode1;
        public string RecordTypeCode2;
        public string TransactionCode1;
        public string TransactionCode2;
        public string EndRecordIndicator1;
        public string EndRecordIndicator2;
        public string OriginatorStatusCode;
        public string RecordTypeCode3;
        public string BatchNumber;
        public string JulianDate;
        
        public int EN { get; set; }
        public DataTable DtBank
        {
            get { return _dtBank; }
            set { _dtBank = value; }
        }
        public DataSet DsBank
        {
            get { return _dsBank; }
            set { _dsBank = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class GetAllBankNamesParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
    }

    public class GetAllBankNamesByCompanyParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int EN { get; set; }
    }

    public class GetBankByIDParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int ID;
    }

    public class GetBankCDParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int ID;
    }

    public class UpdateNextCheckParam
    {
        private string _ConnConfig;
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public int ID;
        public long NextC;
    }

}
