using System;
using System.Security.Cryptography;
using System.Text;
using BusinessEntity.Utility;
using BusinessLayer.Utility;
using Newtonsoft.Json;

namespace MOMWebAPI.Utility
{
    public class UtilityRepository : IUtilityRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pram"></param>
        /// <returns></returns>
        public APIUtility GetAPIUtility(APIRequest _APIRequest)
        {

             
            string _DecryptedToken = Decrypt(_APIRequest.Token);

            string _ConnectionString = GetConnectionString(_DecryptedToken);

            APIUtility _APIUtility = new APIUtility();

            _APIUtility.Token = _APIRequest.Token;

            _APIUtility.Param = Decrypt(_APIRequest.Param);

            _APIUtility.DecryptedToken = _DecryptedToken;

            _APIUtility.ConnectionString = _ConnectionString;

            return _APIUtility;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        public string GetConnectionString(string Token)
        {
           

            string dbname  = Token.Split("|||")[1].ToString();

            string server = Startup.connectionString.Split(';')[0].Split('=')[1];

            string database = dbname;

            string user = Startup.connectionString.Split(';')[2].Split('=')[1];

            string pass = Startup.connectionString.Split(';')[3].Split('=')[1];

            string constr = "server=" + server + ";database=" + database + ";user=" + user + ";password=" + pass + "";

            return constr;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strEncrypted"></param>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public  string Decrypt(string strEncrypted  )
        {
            try
            {
                string strKey = "core";
                TripleDESCryptoServiceProvider objDESCrypto = new TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider objHashMD5 = new MD5CryptoServiceProvider();

                byte[] byteHash, byteBuff;
                string strTempKey = strKey;

                byteHash = objHashMD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(strTempKey));
                objHashMD5 = null;
                objDESCrypto.Key = byteHash;
                objDESCrypto.Mode = CipherMode.ECB; //CBC, CFB

                byteBuff = Convert.FromBase64String(strEncrypted);
                string strDecrypted = ASCIIEncoding.ASCII.GetString(objDESCrypto.CreateDecryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));
                objDESCrypto = null;

                return strDecrypted;
            }
            catch (Exception ex)
            {
                return "Wrong Input. " + ex.Message;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strToEncrypt"></param>
        /// <returns></returns>
        public  string Encrypt(string strToEncrypt)
        {
            try
            {

                string strKey = "core";

                TripleDESCryptoServiceProvider objDESCrypto = new TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider objHashMD5 = new MD5CryptoServiceProvider();

                byte[] byteHash, byteBuff;
                string strTempKey = strKey;

                byteHash = objHashMD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(strTempKey));
                objHashMD5 = null;
                objDESCrypto.Key = byteHash;
                objDESCrypto.Mode = CipherMode.ECB; //CBC, CFB

                byteBuff = ASCIIEncoding.ASCII.GetBytes(strToEncrypt);
                return Convert.ToBase64String(objDESCrypto.CreateEncryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));
            }
            catch (Exception ex)
            {
                return "Wrong Input. " + ex.Message;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_Core_Session_Data"></param>
        /// <returns></returns>
           //public int InsertSessionData(Core_Session_Data _Core_Session_Data, string ConnectionString ) {            
        //  new BL_Utility().ADD_Updatet_Core_Session_Data(_Core_Session_DataParam, ConnectionString);           
        //    return 0;
        //}
        //public int DeleteSessionData(Core_Session_Data _Core_Session_Data, string ConnectionString )
        //{
        //    return 1;
        //}
        //public int UpdateSessionData(Core_Session_Data _Core_Session_Data, string ConnectionString )
        //{
        //    new BL_Utility().ADD_Updatet_Core_Session_Data(_Core_Session_DataParam, ConnectionString);
        //    return 1;
        //}
        //public Core_Session_Data GetSessionData(Core_Session_Data _Core_Session_Data, string ConnectionString )
        //{
        //    return new BL_Utility().SpGet_Core_Session_Data(_Core_Session_DataParam, ConnectionString);              
        //}
    }
}
