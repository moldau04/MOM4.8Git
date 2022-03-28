using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class EmailTemplate
    {
        public string ConnConfig { get; set; }
        public int Id { get; set; }
        public string Screen { get; set; }
        public string FunctionName { get; set; }
        public string Template { get; set; }
        public string UserName { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
}
