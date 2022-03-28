using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class CollectionContacts
    {
        public Int32 ID { get; set; }
        public String Name { get; set; }
        public String Phone { get; set; }
        public String Fax { get; set; }
        public String Cell { get; set; }
        public String Email { get; set; }
        public String Title { get; set; }
        public Boolean Tickets { get; set; }
        public Boolean InvoiceStatements { get; set; }
        public Boolean Shutdown { get; set; }
        public Boolean Tests { get; set; }

        public String CType { get; set; }

        public Int32 LocID { get; set; }

        public Int32 CustID { get; set; }

        public String ConnConfig { get; set; }

        public Boolean IsUpdate { get; set; }

    }
}
