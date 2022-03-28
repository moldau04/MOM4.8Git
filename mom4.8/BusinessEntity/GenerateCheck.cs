using System;
using System.Data;

namespace BusinessEntity
{
    public class GenerateCheck
    {
        public int CheckNum;
        public DateTime CheckDate;
        public Double TotalAmount;
        public string TotalAmountWords;
        public string VendorName;
        public string VendorAddress;
        public DataTable dtOpenAP;
    }
}
