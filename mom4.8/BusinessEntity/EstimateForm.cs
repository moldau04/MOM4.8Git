using System;
using System.Data;

namespace BusinessEntity
{
    public class EstimateForm
    {
        public int Id {get;set;}
        public int Estimate { get; set; }
        public int JobTID { get; set; }
        public string Name {get;set;}
	    public string FileName {get;set;}
        public string FilePath { get; set; }
        public string PdfFilePath { get; set; }
        public string Remark { get; set; }
	    public string MIME  {get;set;}
        public string UpdatedBy {get;set;}
        
        public string AddedOn { get; set; }
        public DateTime UpdatedOn { get; set;}
        public string ConnConfig{get;set;}
        public DataSet ds {get;set;}
    
    }
}
