using BusinessEntity.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOMWebAPI.Utility
{
    public interface IUserAuthenticationRepository
    {
        public UserAuthentication GetUserAuthentication(  APIUtility _APIUtility);
    }
}
