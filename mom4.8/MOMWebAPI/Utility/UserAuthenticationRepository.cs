using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BusinessEntity.Utility;
using Microsoft.ApplicationBlocks.Data;

namespace MOMWebAPI.Utility
{
    public class UserAuthenticationRepository : IUserAuthenticationRepository
    {
        public UserAuthentication GetUserAuthentication(APIUtility _APIUtility)
        {

            UserAuthentication _UA = new UserAuthentication();

            _UA.Connectionstring = _APIUtility.ConnectionString;
            _UA.Token = _APIUtility.Token;
            _UA.company = _APIUtility.DecryptedToken.Split("|||")[1].ToString();
            _UA.Domain_Name = _APIUtility.DecryptedToken.Split("|||")[2].ToString();
            _UA.User_Id = Convert.ToInt32(_APIUtility.DecryptedToken.Split("|||")[3].ToString());
            _UA.IsValid = false;

            try
            {

                SqlParameter[] para = new SqlParameter[4];
                para[0] = new SqlParameter
                {
                    ParameterName = "Token",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _UA.Token
                };

                para[1] = new SqlParameter
                {
                    ParameterName = "Domain_Name",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _UA.Domain_Name
                };

                para[2] = new SqlParameter
                {
                    ParameterName = "User_Id",
                    SqlDbType = SqlDbType.Int,
                    Value = _UA.User_Id
                };

                para[3] = new SqlParameter
                {
                    ParameterName = "company",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _UA.company
                };
               

                int IsUserAuthentication = Convert.ToInt32( SqlHelper.ExecuteScalar(_UA.Connectionstring, CommandType.StoredProcedure, "sp_Core_UserToken" , para));

                if (IsUserAuthentication == 2020) _UA.IsValid = true; 

                return _UA;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            }  
    }
}
