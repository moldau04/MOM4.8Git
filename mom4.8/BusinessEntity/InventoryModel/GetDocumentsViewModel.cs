using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.InventoryModel
{
    [Serializable]
    public class GetDocumentsViewModel
    {
        public int ID { get; set; }
        public string Screen { get; set; }
        public int ScreenID { get; set; }
        public Int16 Line { get; set; }
        public string fDesc { get; set; }
        public string Filename { get; set; }
        public string Path { get; set; }
        public Int16 Type { get; set; }
        public string Remarks { get; set; }
        public DateTime Custom1 { get; set; }
        public DateTime Custom2 { get; set; }
        public DateTime Custom3 { get; set; }
        public DateTime Custom4 { get; set; }
        public DateTime Custom5 { get; set; }
        public Int16 Custom6 { get; set; }
        public Int16 Custom7 { get; set; }
        public Int16 Custom8 { get; set; }
        public Int16 Custom9 { get; set; }
        public Int16 Custom10 { get; set; }
        public string Custom11 { get; set; }
        public string Custom12 { get; set; }
        public string Custom13 { get; set; }
        public string Custom14 { get; set; }
        public string Custom15 { get; set; }
        public string TempID { get; set; }
        public DateTime Date { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public Int16 Portal { get; set; }
        public bool MSVisible { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public DateTime attached_on { get; set; }
        public string doctype { get; set; }
    }
}
