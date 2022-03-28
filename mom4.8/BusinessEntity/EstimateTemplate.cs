using System;
using System.Data;

namespace BusinessEntity
{
    public class EstimateTemplate
    {
        public int Id {get;set;}
        public int JobTID {get;set;}
        public string Name {get;set;}
	    public string FileName {get;set;}
        public string FilePath {get;set;}
	    public string MIME  {get;set;}
        public string AddedBy {get;set;}
        public DateTime AddedOn {get;set;}
        public string UpdatedBy {get;set;}
        public DateTime UpdatedOn { get; set; }
        public string ConnConfig{get;set;}
        public DataSet ds {get;set;}
    
    }
}
