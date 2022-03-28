using System;
using System.Data;
using DataLayer;
using BusinessEntity;

namespace BusinessLayer
{
    public class BL_Contact
    {
        DL_Contact objDL_Contact = new DL_Contact();
        /// <summary>
        /// Get Contact Autojquery
        /// </summary>
        /// <param name="objPropUser"></param>
        /// <param name="IsSalesAsigned"></param>
        /// <returns></returns>
        public DataSet getContactAutojquery(User objPropUser, Int32 IsSalesAsigned = 0)
        {
            return objDL_Contact.getContactAutojquery(objPropUser, IsSalesAsigned);
        }
    }
}
