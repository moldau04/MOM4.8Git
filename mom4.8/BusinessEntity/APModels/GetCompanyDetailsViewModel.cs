using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.APModels
{
    [Serializable]
    public class GetCompanyDetailsViewModel
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Logo { get; set; }
        public string Fax { get; set; }
        public string GSTreg { get; set; }
        public int YE { get; set; }
        public int Version { get; set; }
        public string CDesc { get; set; }
        public string MSM { get; set; }
        public string DSN { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Remarks { get; set; }
        public DateTime BusinessStart { get; set; }
        public DateTime BusinessEnd { get; set; }
    }
}
