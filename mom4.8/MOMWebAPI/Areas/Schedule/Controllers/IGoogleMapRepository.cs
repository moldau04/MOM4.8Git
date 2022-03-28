using MOMWebAPI.Areas.Schedule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOMWebAPI.Areas.Schedule.Controllers
{
  public  interface IGoogleMapRepository
    {
        public List<GPS_LiveData> GetGPSData(string fUser = "", string date = "", string cat = "", string iscall = "", string webconfig = "");

    }
}
