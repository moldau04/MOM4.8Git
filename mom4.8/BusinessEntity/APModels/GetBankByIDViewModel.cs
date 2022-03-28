using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.APModels
{
    [Serializable]
    public class GetBankByIDViewModel
    {
        public int ID { get; set; }
        public string fDesc { get; set; }
        public int Rol { get; set; }
        public string NBranch { get; set; }
        public string NAcct { get; set; }
        public string NRoute { get; set; }
        public Int64 NextC { get; set; }
        public int NextD { get; set; }
        public int NextE { get; set; }
        public double Rate { get; set; }
        public double CLimit { get; set; }
        public Int16 Warn { get; set; }
        public double Recon { get; set; }
        public double Balance { get; set; }
        public Int16 Status { get; set; }
        public Int16 InUse { get; set; }
        public string ACHFileHeaderStringA { get; set; }
        public string ACHFileHeaderStringB { get; set; }
        public string ACHFileHeaderStringC { get; set; }
        public string ACHCompanyHeaderString1 { get; set; }
        public string ACHCompanyHeaderString2 { get; set; }
        public string ACHBatchControlString1 { get; set; }
        public string ACHBatchControlString2 { get; set; }
        public string ACHBatchControlString3 { get; set; }
        public string ACHFileControlString1 { get; set; }
        public int Chart { get; set; }
        public DateTime LastReconDate { get; set; }
        public double BankBalance { get; set; }
        public Int64 NextCash { get; set; }
        public Int64 NextWire { get; set; }
        public Int64 NextACH { get; set; }
        public Int64 NextCC { get; set; }
        public int BankType { get; set; }
        public int ChartID { get; set; }
        public string APACHCompanyID { get; set; }
        public string APImmediateOrigin { get; set; }
    }
}
