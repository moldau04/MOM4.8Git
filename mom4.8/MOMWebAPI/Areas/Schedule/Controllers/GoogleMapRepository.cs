using Microsoft.ApplicationBlocks.Data;
using MOMWebAPI.Areas.Schedule.Models;
using System;
using System.Collections.Generic;
 

namespace MOMWebAPI.Areas.Schedule.Controllers
{
    public class GoogleMapRepository : IGoogleMapRepository
    {

        public List<GPS_LiveData> GetGPSData(string fUser = "",  string date = "", string cat = "", string iscall = "",  string webconfig = "" )
        {
            try
            {
                List<GPS_LiveData> _GPS = new List<GPS_LiveData>();
                SqlHelper.ExecuteDataset(webconfig, "spGetGPSData", fUser, date, cat, iscall);
                return _GPS;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
