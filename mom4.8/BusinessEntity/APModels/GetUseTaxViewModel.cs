using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.APModels
{
    [Serializable]
    public class GetUseTaxViewModel
    {
        public DateTime PJfDate { get; set; }
        public string PJRef { get; set; }
        public int PJBatch { get; set; }
        public double PJItemAmount { get; set; }
        public string RolName { get; set; }
        public string STaxName { get; set; }
        public double STaxRate { get; set; }
        public int TransBatch { get; set; }
        public DateTime TransfDate { get; set; }
        public string TransfDesc { get; set; }
        public string TransType { get; set; }
        public double TransAmount { get; set; }
    }
}
