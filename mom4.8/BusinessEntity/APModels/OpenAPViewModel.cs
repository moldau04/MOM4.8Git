using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.APModels
{
    [Serializable]
    public class OpenAPViewModel
    {
        public int Vendor{ get; set; }
        public DateTime fDate{ get; set; }
        public DateTime Due{ get; set; }
        public Int16 Type{ get; set; }
        public string fDesc{ get; set; }
        public double Original{ get; set; }
        public double Balance{ get; set; }
        public double Selected{ get; set; }
        public double Disc{ get; set; }
        public int PJID{ get; set; }
        public int TRID{ get; set; }
        public string Ref{ get; set; }
        public DateTime SearchDate{ get; set; }
        public Int16 SearchValue{ get; set; }

        private DataSet _ds;
        private string _ConnConfig;

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
        public int IsSelected{ get; set; }

        public double RunningBalance{ get; set; }
        public Int32 Counts{ get; set; }
        public Int32 TotVendor{ get; set; }
        public string RolName{ get; set; }
        public double Discount{ get; set; }
        public Int16 Status{ get; set; }
        public Int16 Spec{ get; set; }
        public string StatusName{ get; set; }
        public double Payment{ get; set; }
        public string billDesc{ get; set; }
        public double Duepayment{ get; set; }
        public Int32 NCount{ get; set; }
        public double NAmt{ get; set; }

        //GetPurchaseJournal API
        public int ID{ get; set; }
        public DateTime Post{ get; set; }
        public double WeekCount{ get; set; }
        public DateTime WeekDate{ get; set; }
        public double Amount{ get; set; }
        public string VendorName { get; set; }
        public string VendorType { get; set; }
    }
}
