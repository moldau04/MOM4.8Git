using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Payroll
{
    [Serializable]
    public class HeaderFooterDetailViewModel
    {
        public int Id { get; set; }
        public int ReportId { get; set; }
        public bool MainHeader { get; set; } 
        public string CompanyName { get; set; }
        public string ReportTitle { get; set; }
        public string SubTitle { get; set; }
        public string DatePrepared { get; set; }
        public bool TimePrepared { get; set; }
        public bool ReportBasis { get; set; }
        public string PageNumber { get; set; }
        public string ExtraFooterLine { get; set; }
        public string Alignment { get; set; }
        public string PDFSize { get; set; }
    }
}
