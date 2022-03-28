using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.APModels
{
    [Serializable]
    public class BankViewModel
    {
        public int ID{ get; set; }
        public string fDesc{ get; set; }
        public int Rol{ get; set; }
        public string NBranch{ get; set; }
        public string NAcct{ get; set; }
        public string NRoute{ get; set; }
        public int NextC{ get; set; }
        public int NextD{ get; set; }
        public int NextE{ get; set; }
        public double Rate{ get; set; }
        public double CLimit{ get; set; }
        public Int16 Warn{ get; set; }
        public double Recon{ get; set; }
        public double Balance{ get; set; }
        public int Status{ get; set; }
        public int InUse{ get; set; }
        public int GeoLock = 0;
        private DataSet _dsBank;
        private string _ConnConfig;
        public int Chart{ get; set; }
        public DateTime LastReconDate{ get; set; }
        public double ServiceCharge{ get; set; }
        public double InterestCharge{ get; set; }
        public int ServiceAcct{ get; set; }
        public int InterestAcct{ get; set; }
        public DateTime ServiceDate{ get; set; }
        public DateTime InterestDate{ get; set; }
        public DataTable _dtBank;
        public DateTime fDate{ get; set; }
        public int EN { get; set; }
        public int NextCash { get; set; }
        public int NextWire { get; set; }
        public int NextACH { get; set; }
        public int NextCC { get; set; }

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

        public string Company{ get; set; }
        public string ACHFileHeaderStringA{ get; set; }
        public string ACHFileHeaderStringB { get; set; }
        public string ACHFileHeaderStringC{ get; set; }
        public string ACHCompanyHeaderString1{ get; set; }
        public string ACHCompanyHeaderString2{ get; set; }
        public string ACHBatchControlString1{ get; set; }
        public string ACHBatchControlString2{ get; set; }
        public string ACHBatchControlString3{ get; set; }
        public string ACHFileControlString1{ get; set; }
        public double BankBalance{ get; set; }
        public string RolName{ get; set; }
        public string RolAddress{ get; set; }
        public string RolState{ get; set; }
        public string RolCity{ get; set; }
        public string RolZip{ get; set; }

        //New fields
        public string APACHCompanyID { get; set; }
        public string APImmediateOrigin { get; set; }
        public int BankType { get; set; }
        public int ChartID { get; set; }

    }
}
