using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.APModels
{
    [Serializable]
    public class RolViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip{ get; set; }
        public string Phone{ get; set; }
        public string Fax{ get; set; }
        public string Contact{ get; set; }
        public string Remarks{ get; set; }
        public int Type{ get; set; }
        public int fLong{ get; set; }
        public int Latt{ get; set; }
        public int GeoLock{ get; set; }
        public DateTime Since{ get; set; }
        public DateTime Last{ get; set; }
        public string Address{ get; set; }
        public int EN{ get; set; }
        public string EMail{ get; set; }
        public string Website{ get; set; }
        public string Cellular{ get; set; }
        public string Category{ get; set; }
        public string Position{ get; set; }
        public string Country{ get; set; }
        public string lat{ get; set; }
        public string lng{ get; set; }
        public DateTime LastUpdateDate{ get; set; }
        public string ConnConfig{ get; set; }

        public Bank objBank{ get; set; }
        private DataSet _dsRol;
        private DataSet _dsID;

        public DataSet DsRol
        {
            get { return _dsRol; }
            set { _dsRol = value; }
        }
        public DataSet DsID
        {
            get { return _dsID; }
            set { _dsID = value; }
        }
        public string MOMUSer { get; set; }
    }
}
