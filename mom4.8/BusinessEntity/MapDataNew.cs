using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    [Serializable]
    public class MapDataNew
    {
        private DataTable _LocData;

        public DataTable LocData
        {
            get { return _LocData; }
            set { _LocData = value; }
        }

        public string deviceId { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public DateTime date { get; set; }
        public int ID { get; set; }
        public DateTime SysDate { get; set; }
        public int fake { get; set; }
        public string Accuracy { get; set; }
        public string fuser { get; set; }
        public string userId { get; set; }





    }
}
