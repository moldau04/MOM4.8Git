using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.CustomersModel
{
    [Serializable]
    public class GetLocationDocumentsViewModel
    {
        public string Filename { get; set; }
        public string doctype { get; set; }
        public int Project { get; set; }
        public string ProjectName { get; set; }
        public int Ticket { get; set; }
        public string AssignedTo { get; set; }
        public int Date { get; set; }
        public string Path { get; set; }
        public string Screen { get; set; }
        public string Remarks { get; set; }
        public int ID { get; set; }
        public Int16 Portal { get; set; }
    }
}
