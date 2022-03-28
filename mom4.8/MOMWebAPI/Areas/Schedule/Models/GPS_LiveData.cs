using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOMWebAPI.Areas.Schedule.Models
{
    public class GPS_LiveData
    {         
        public string MAPID { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string date { get; set; }
        public string timestm { get; set; }
        public string indexID { get; set; }
        public string Cat { get; set; }
           

    }


    public class GPSLiveDataPram
    {
        public string webconfig { get; set; }
        public string fUser { get; set; }
        public string date { get; set; }
        public string cat { get; set; }
        public string iscall { get; set; } 

    } 
}
