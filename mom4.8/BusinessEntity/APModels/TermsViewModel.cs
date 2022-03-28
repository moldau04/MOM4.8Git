using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.APModels
{
    [Serializable]
    public class TermsViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string QBTermsID;
        public DateTime LastUpdateDate;
        private DataSet _dstblTerms;
        public DataSet DsTerms
        {
            get { return _dstblTerms; }
            set { _dstblTerms = value; }
        }
    }
}
