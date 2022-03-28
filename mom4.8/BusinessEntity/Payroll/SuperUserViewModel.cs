using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    [Serializable]
    public class SuperUserViewModel
    {
        public int ID {get;set;}
        public string fFirst { get; set; }

        public string Last { get; set; }
        public int Status { get; set; }
        public int userid { get; set; }
        public string fUser { get; set; }
        public string super { get; set; }
        public string usertype { get; set; }
        public int usertypeid { get; set; }
        public string userkey { get; set; }

    }
}
