using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.APModels
{
    [Serializable]
    public class GetReceivePOListViewModel
    {
        public int ID { get; set; }
        public int Value { get; set; }
        public double ReceivedAmount { get; set; }
        public DateTime ReceiveDate { get; set; }
    }
}
