using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessEntity.Utility;

namespace MOMWebAPI.Utility
{
     public interface IUtilityRepository
    {
        APIUtility GetAPIUtility(APIRequest _APIRequest);

        public string Decrypt(string strEncrypted);

        public  string Encrypt(string strToEncrypt);

        //public int InsertSessionData(Core_Session_Data _Core_Session_Data , string ConnectionString);

        //public string GetSessionData(Core_Session_Data _Core_Session_Data , string ConnectionString);

        //public int DeleteSessionData(Core_Session_Data _Core_Session_Data , string ConnectionString);

        //public int UpdateSessionData(Core_Session_Data _Core_Session_Data , string ConnectionString);
    }
}
