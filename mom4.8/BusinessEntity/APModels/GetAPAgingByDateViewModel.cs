using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.APModels
{
    [Serializable]
    public class GetAPAgingByDateViewModel
    {
        public int VendorID{get;set;}
        public int PJID{get;set;}
        public string Vendor{get;set;}
        public DateTime fDate{get;set;}//datetime
        public DateTime Due{get;set;}//datetime
        public string Ref{get;set;}
        public string fDesc{get;set;}
        public int TRID{get;set;}
        public double Original{get;set;}
        public double Paid{get;set;}
        public double Total{get;set;}       
        public Int32 DueIn{get;set;}
        public double SevenDay{get;set;}
        public double ThirtyDay{get;set;}
        public double SixtyDay{get;set;}
        public double NintyDay{get;set;}
        public double NintyOneDay{get;set;}
        public int isIssueDate{get;set;}








    }
}
