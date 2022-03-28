using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.APModels
{
    [Serializable]
    public class GetUTaxLocReportViewModel
    {
        public string vendor{get; set;}
        public double Amount{get; set;}
        public int PO{get; set;}
        public string PJRef{get; set;}
        public int JobID{get; set;}
        public double PJItemAmount{get; set;}
        public string LocationName{get; set;}
        public string JobType{get; set;}
        public string Descp{get; set;}
        public DateTime PJfDate{get; set;}
        public string STaxName{get; set;}
        public string State{get; set;}
        public string TaxDesc{get; set;}
        public double STaxRate{get; set;}
        public int TransBatch{get; set;}
        public DateTime TransfDate{get; set;}
        public string LineItemDesc{get; set;}
        public string TransType{get; set;}
        public string StatusName{get; set;}
    }
}
