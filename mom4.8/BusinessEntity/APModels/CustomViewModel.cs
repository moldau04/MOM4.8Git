using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.APModels
{
    [Serializable]
    public class CustomViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public int Number { get; set; }
        public double GstRate { get; set; }
    }
}
