using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class TeamMemberTitle
    {
        public string ConnConfig { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Remarks { get; set; }
        public bool IsDefault { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public int OrderNo { get; set; }
    }
}
