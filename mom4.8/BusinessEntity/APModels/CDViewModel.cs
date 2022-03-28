using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.APModels
{
    [Serializable]
    public class CDViewModel
    {
        public int ID{ get; set; }
        public DateTime fDate{ get; set; }
        public int Ref{ get; set; }
        public string fDesc{ get; set; }
        
        public double Amount{ get; set; }
        public int Bank{ get; set; }
        public Int16 Type{ get; set; }
        public Int16 Status{ get; set; }
        public int TransID{ get; set; }
        public int Vendor{ get; set; }
        public string French{ get; set; }
        public string Memo{ get; set; }
        public string VoidR{ get; set; }
        public Int16 ACH{ get; set; }
        public int fDateYear{ get; set; }
        public bool IsRecon{ get; set; }
        public DateTime StartDate{ get; set; }
        public DateTime EndDate{ get; set; }
        public int NextC{ get; set; }
        private DataSet _ds;
        private string _ConnConfig;
        public bool IsExistCheckNo{ get; set; }
        private DataTable _dt;
        public int DiscGL{ get; set; }
        public string searchterm{ get; set; }
        public string searchvalue{ get; set; }
        public string updateBy{ get; set; }
        public DateTime updateByValue{ get; set; }
        public bool isVH{ get; set; }
        public bool isDisc{ get; set; }
   
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
        public int Sel { get; set; }
        public string BankName{ get; set; }
        public int Batch { get; set; }
        public string VendorName { get; set; }
        public string Acct { get; set; }
        public string Company { get; set; }
        public Int32 CountRecur{ get; set; }

        public string COLUMN_NAME{ get; set; }
        public string DATA_TYPE{ get; set; }
        public string StatusName { get; set; }
        public string TypeName { get; set; }
        public int TotalRow { get; set; }
    }

}
