using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class EmailSignature
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }
        public bool IsDefault { get; set; }
        public string ConnConfig { get; set; }
    }
}
