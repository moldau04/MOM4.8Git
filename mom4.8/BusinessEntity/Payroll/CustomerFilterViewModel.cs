using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    [Serializable]
    public class CustomerFilterViewModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string ColumnName { get; set; }
        public string FilterColumn { get; set; }
        public string FilterSet { get; set; }
        public string Contact { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string Website { get; set; }
        public string Cellular { get; set; }
        public int loc { get; set; }
        public int equip { get; set; }
        public int opencall { get; set; }

        public string Balance { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string WebAddress { get; set; }
        public string Logo { get; set; }
        public string DBName { get; set; }
       

    }
}
