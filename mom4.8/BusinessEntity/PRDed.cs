using System;
using System.Data;

namespace BusinessEntity
{
    public class PRDed
    {
        private string _ConnConfig;
        private string _DBName;
        public DataSet _ds;
        public DataTable _dt;
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
        public int ID { get; set; }

        public string  fDesc { get; set; }
        public int Type { get; set; }
        public int ByW { get; set; }
        public int BasedOn { get; set; }
        public int AccruedOn { get; set; }
        public int Count { get; set; }
        public double EmpRate { get; set; }
        public double EmpTop { get; set; }
        public int EmpGL { get; set; }
        public double CompRate { get; set; }
        public double CompTop { get; set; }
        public double CompGL { get; set; }
        public int CompGLE { get; set; }
        public int Paid { get; set; }

        public int Vendor { get; set; }
        public double Balance { get; set; }
        public int InUse { get; set; }

        public string Remarks { get; set; }
        public int DedType { get; set; }
        public int Reimb { get; set; }
        public int Job { get; set; }
        public int Box { get; set; }
        public int Frequency { get; set; }
        public int Process { get; set; }
        public string SearchValue { get; set; }
        public int VertexDeductionId { get; set; }

    }
}
